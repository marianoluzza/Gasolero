using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Cierre : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Cierres"; } }
		public static string NombreEntidad
		{ get { return "Cierre"; } }
		public static string NombreClave
		{ get { return "IdCierre"; } }

		#endregion

		#region Atributos y Propiedades
		System.Decimal _Costo;
		public System.Decimal Costo
		{
			get { return _Costo; }
			set { _Costo = value; }
		}

		System.Decimal _Entrada;
		public System.Decimal Entrada
		{
			get { return _Entrada; }
			set { _Entrada = value; }
		}

		System.UInt32 _IdCierre;
		public System.UInt32 IdCierre
		{
			get { return _IdCierre; }
			set { _IdCierre = value; }
		}

		System.UInt32 _IdVendedor;
		public System.UInt32 IdVendedor
		{
			get { return _IdVendedor; }
			set { _IdVendedor = value; }
		}

		System.Decimal _Salida;
		public System.Decimal Salida
		{
			get { return _Salida; }
			set { _Salida = value; }
		}

		System.UInt32 _Surtidor;
		public System.UInt32 Surtidor
		{
			get { return _Surtidor; }
			set { _Surtidor = value; }
		}

		System.UInt32 _Turno;
		/// <summary>
		/// Numero de turno al que corresponde (no id)
		/// </summary>
		public System.UInt32 Turno
		{
			get { return _Turno; }
			set { _Turno = value; }
		}

		System.UInt32 _IdArticulo;
		public System.UInt32 IdArticulo
		{
			get { return _IdArticulo; }
			set { _IdArticulo = value; }
		}

		System.UInt32 _IdParcial = 0;
		public System.UInt32 IdParcial
		{
			get { return _IdParcial; }
			set { _IdParcial = value; }
		}
		#endregion

		#region Constructores
		public Cierre() { }

		public Cierre(DataRow r)
		{
			Costo = decimal.Parse(r["Costo"].ToString());
			Entrada = decimal.Parse(r["Entrada"].ToString());
			IdCierre = uint.Parse(r["IdCierre"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			Salida = decimal.Parse(r["Salida"].ToString());
			Surtidor = uint.Parse(r["Surtidor"].ToString());
			Turno = uint.Parse(r["Turno"].ToString());
			IdArticulo = uint.Parse(r["IdArticulo"].ToString());
			IdParcial = uint.Parse(r["IdParcial"].ToString());
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Cierres);
		}

		public decimal GetDiferencia()
		{
			decimal dif = Salida - Entrada;
			if (dif < 0)
				dif += Reglas.Puestos[Surtidor - 1].Ciclo;
			return dif;
		}

		public enTipoMargenViolado VigilarMargen()
		{
			decimal dif = GetDiferencia();
			if (dif > Reglas.Puestos[Surtidor - 1].Margen)
				return enTipoMargenViolado.MaxMargenMayor;
			else if (dif == 0)
				return enTipoMargenViolado.Cero;
			else if (Reglas.ModoMargen != enModoMargen.Ninguno)
			{ //ver margen por error menor, si existe el chequeo
				decimal difParcialAux = 0, difParcialAuxMin = int.MaxValue, difParcialAuxMax = 0;
				//ver tantos turnos como sea pedido
				int ctTurnosAVer = Math.Max(1, Reglas.TurnosParaPromedio);
				for (int i = 0; i < ctTurnosAVer; i++)
				{
					foreach (var cierreParcialAux in Cierre.GetListaXSurtidorTurno(Surtidor,
						(uint)(Reglas.TurnoUltimo.Numero + 1 - (3 * i))))//-3 ver los turnos iguales M T Noche
					{
						if (Reglas.ModoMargen == enModoMargen.Promedio)
							difParcialAux += cierreParcialAux.GetDiferencia();
						else if (Reglas.ModoMargen == enModoMargen.MinYMax)
						{
							difParcialAux = cierreParcialAux.GetDiferencia();
							//dif parcial obtenida
							difParcialAuxMax = Math.Max(difParcialAux, difParcialAuxMax);
							if (difParcialAux > 0)
								difParcialAuxMin = Math.Min(difParcialAux, difParcialAuxMin);
						}
					}//fin de ese turno
				}
				if (Reglas.ModoMargen == enModoMargen.Promedio)
				{
					difParcialAuxMax = difParcialAux / ctTurnosAVer;
					difParcialAuxMin = difParcialAuxMax;
				}
				else if (difParcialAuxMin == int.MaxValue)
				{
					difParcialAuxMin = 0;
				}
				//
				if (dif > difParcialAuxMax * (1.0M + decimal.Divide(Reglas.MargenSup, 100)))//se paso del margen mayor
					return enTipoMargenViolado.SupMayor;
				else if (dif < difParcialAuxMin * (1.0M - decimal.Divide(Reglas.MargenSup, 100)))//se quedo corto del margen mayor
					return enTipoMargenViolado.SupMenor;
				else if (dif > difParcialAuxMax * (1.0M + decimal.Divide(Reglas.MargenInf, 100)))//se paso del margen menor, sin llegar al mayor
					return enTipoMargenViolado.InfMayor;
				else if (dif < difParcialAuxMin * (1.0M - decimal.Divide(Reglas.MargenInf, 100)))//se quedó corto del margen menor, sin llegar al mayor
					return enTipoMargenViolado.InfMenor;
			}
			return enTipoMargenViolado.Ninguno;
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.Cierres);
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.Cierres, NombreClave, id);
		}

		public static Cierre GetSingleE(uint id)
		{
			return new Cierre(GetSingle(id).Rows[0]);
		}

		public static Cierre GetUltimo(uint surtidor)
		{
			DataTable dtCierres = Datos.CierresGetUltimo(surtidor);
			if (dtCierres.Rows.Count > 0)
				return new Cierre(dtCierres.Rows[0]);
			else
			{
				Cierre res = new Cierre();
				res._Costo = 0;
				res._Entrada = 0;
				res._IdCierre = 0;
				res._IdVendedor = 1;
				res._Salida = 0;
				res._Surtidor = surtidor;
				res._Turno = Reglas.TurnoUltimo.Numero;
				return res;
			}
		}

		public static Cierre[] GetListaXSurtidorTurno(uint surtidor, uint turno)
		{
			Cierre[] res;
			DataTable dt = Datos.CierresGetListForSurtidorTurno(surtidor, turno);
			res = new Cierre[dt.Rows.Count];
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				res[i] = new Cierre(dt.Rows[i]);
			}
			return res;
		}

		public static Cierre[] GetListaXArticuloTurno(uint articulo, uint turno)
		{
			Cierre[] res;
			DataTable dt = Datos.CierresGetListForIdArticuloTurno(articulo, turno);
			res = new Cierre[dt.Rows.Count];
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				res[i] = new Cierre(dt.Rows[i]);
			}
			return res;
		}

		public static Cierre[] GetListaXParcial(uint turno, uint idParcial)
		{
			Cierre[] res;
			DataTable dt = Datos.CierresGetListForIdParcial(turno, idParcial);
			res = new Cierre[dt.Rows.Count];
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				res[i] = new Cierre(dt.Rows[i]);
			}
			return res;
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.Cierres, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdCierre = res.IdInsercion;
			return res;
		}

		public ABMResultado ModificarSalida(decimal nvaSalida, out FactEncabezado[] factDif)
		{
			factDif = null;
			List<FactEncabezado> factList = new List<FactEncabezado>();
			ABMResultado res;
			bool exito = false;
			bool empeceTransa = DAL.Datos.InitTransa();
			try
			{
				Turno t = Negocio.Turno.GetForNumero(Turno);
				decimal dif = nvaSalida - Salida;
				for (decimal resto = dif; resto != 0; )
				{
					FactEncabezado fact = new FactEncabezado(t.FechaFinal.AddSeconds(-1));
					fact.IdCliente = Cliente.ConsFinal.Id;
					FactItem it = new FactItem();
					Articulo art = Articulo.GetSingleE(Reglas.Puestos[Surtidor - 1].IdArticulo);
					it.IdArticulo = art.IdArticulo;
					it.DescPlana = art.Descripcion;
					it.Precio = art.GetPrecioBase();
					it.Impuestos = it.Precio * art.GetImpInt();
					it.IVA = it.Precio * art.Alicuota / 100;
					it.Monto = Math.Min(FactEncabezado.GetTotalMaximo(), art.Precio * resto);
					it.Cantidad = decimal.Divide(it.Monto, art.Precio);
					resto -= it.Cantidad;
					fact.Cuerpo.Add(it);
					factList.Add(fact);
					res = fact.Alta();
					if (res.CodigoError != enErrores.Ninguno)
						return res;
				}
				if (dif == 0)
					return new ABMResultado(enErrores.Cancelado, "No es necesario guardar la diferencia");
				Salida = nvaSalida;
				res = DAL.Datos.ABM(DAL.enTablas.Cierres, enABMFuncion.Modificacion, ProtocoloDatos());
				if (res.CodigoError == enErrores.Ninguno)
					exito = true;
				return res;
			}
			catch (Exception ex)
			{
				exito = false;
				factList.Clear();
				return new ABMResultado(enErrores.ErrorBD, ex.Message);
			}
			finally
			{
				if (empeceTransa)
					DAL.Datos.FinTransa(exito);
				factDif = factList.ToArray();
			}
		}

		public override ABMResultado Modificacion()
		{
			ABMResultado res;
			bool exito = false;
			bool empeceTransa = DAL.Datos.InitTransa();
			try
			{
				res = DAL.Datos.ABM(DAL.enTablas.Cierres, enABMFuncion.Modificacion, ProtocoloDatos());
				if (res.CodigoError == enErrores.Ninguno)
					exito = true;
				return res;
			}
			catch (Exception ex)
			{
				exito = false;
				return new ABMResultado(enErrores.ErrorBD, ex.Message);
			}
			finally
			{
				if (empeceTransa)
					DAL.Datos.FinTransa(exito);
			}
		}

		public override ABMResultado Baja()
		{
			return DAL.Datos.ABM(DAL.enTablas.Cierres, enABMFuncion.Baja, ProtocoloDatos());
		}
		#endregion
	}
}