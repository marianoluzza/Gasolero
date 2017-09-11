using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Turno : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Turnos"; } }
		public static string NombreEntidad
		{ get { return "Turno"; } }
		public static string NombreClave
		{ get { return "IdTurno"; } }

		#endregion

		#region Atributos y Propiedades
		System.DateTime _FechaFinal;
		public System.DateTime FechaFinal
		{
			get { return _FechaFinal; }
			set { _FechaFinal = value; }
		}

		System.UInt32 _IdTurno;
		public System.UInt32 IdTurno
		{
			get { return _IdTurno; }
			set { _IdTurno = value; }
		}

		System.UInt32 _IdVendedor;
		public System.UInt32 IdVendedor
		{
			get { return _IdVendedor; }
			set { _IdVendedor = value; }
		}

		System.UInt32 _Numero;
		public System.UInt32 Numero
		{
			get { return _Numero; }
			set { _Numero = value; }
		}

		System.Decimal _Propina = 0;
		public System.Decimal Propina
		{
			get { return _Propina; }
			set { _Propina = value; }
		}

		#endregion

		#region Constructores
		public Turno() { }

		public Turno(DataRow r)
		{
			FechaFinal = DateTime.Parse(r["FechaFinal"].ToString());
			IdTurno = uint.Parse(r["IdTurno"].ToString());
			Numero = uint.Parse(r["Numero"].ToString());
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			Propina = decimal.Parse(r["Propina"].ToString());
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Turnos);
		}

		public override string ToString()
		{
			return "Turno " + GetTurnoNbre(Numero) + " (" + Numero + ") " + FechaFinal;
		}

		public static string GetTurnoNbre(uint numero)
		{
			enTurnos tur = (enTurnos)(numero % 3);
			switch (tur)
			{
				case enTurnos.Maniana:
					return "Mañana";
				default:
					return tur.ToString();
			}
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.Turnos);
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.Turnos, NombreClave, id);
		}

		public static Turno GetSingleE(uint id)
		{
			return new Turno(GetSingle(id).Rows[0]);
		}

		public static Turno GetUltimo()
		{
			return new Turno(Datos.TurnosGetUltimo().Rows[0]);
		}

		public static Turno GetForNumero(uint numero)
		{
			DataTable dt = Datos.TurnosGetForNumero(numero);
			if (dt.Rows.Count > 0)
				return new Turno(dt.Rows[0]);
			else
				return null;
		}

		public Turno GetForNumero()
		{
			DataTable dt = Datos.TurnosGetForNumero(this.Numero);
			if (dt.Rows.Count > 0)
				return new Turno(dt.Rows[0]);
			else
				return null;
		}

		public static DataTable GetForFechas(DateTime dde, DateTime hta)
		{
			DataTable dt = Datos.TurnosGetForDesdeHasta(dde, hta);
			return dt;
		}

		public static DataTable GetEntregasForFechas(DateTime dde, DateTime hta)
		{
			DataTable dt = Datos.TurnosEntregadosGetForDesdeHasta(dde, hta);
			return dt;
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.Turnos, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdTurno = res.IdInsercion;
			return res;
		}

		public override ABMResultado Modificacion()
		{
			return DAL.Datos.ABM(DAL.enTablas.Turnos, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			return DAL.Datos.ABM(DAL.enTablas.Turnos, enABMFuncion.Baja, ProtocoloDatos());
		}

		public static ABMResultado CerrarTurno(out FactEncabezado[] difs, Cierre[] cierres, decimal propina, ValeEntrega[] vales)
		{
			Turno turno = Reglas.TurnoUltimo;
			Parcial parcial = null;
			bool empeceTransa = Datos.InitTransa();
			bool exito = false;
			difs = new FactEncabezado[0];
			SortedList<uint, int> cierresXArt = new SortedList<uint, int>(10);
			ABMResultado abmRes;
			try
			{
				//si es parcial, anoto el cierre parcial para luegon indicarlo en los cierres
				if (cierres.Length < Reglas.CantidadPuestos)
				{
					//ahora el cierre parcial tambien se anota
					parcial = new Parcial();
					parcial.Turno = turno.Numero + 1;
					parcial.Fecha = Reglas.Now.AddSeconds(1);
					abmRes = parcial.Alta();
					if (abmRes.CodigoError != enErrores.Ninguno)
						return abmRes;
				}
				//esto se hace siempre independiente del tipo de cierre
				foreach (Cierre c in cierres)
				{
					c.IdParcial = parcial == null ? 0 : parcial.IdParcial;
					abmRes = c.Alta();
					if (abmRes.CodigoError != enErrores.Ninguno)
						return abmRes;
					uint idArt = Reglas.Puestos[c.Surtidor - 1].IdArticulo;
					if (!cierresXArt.ContainsKey(idArt))
						cierresXArt.Add(idArt, 1);
					else
						cierresXArt[idArt]++;
				}
				//es cierre parcial?
				if (cierres.Length < Reglas.CantidadPuestos)
				{
					//cuantos puestos cerró x art?
					foreach (Puesto p in Reglas.Puestos)
					{
						uint idArt = p.IdArticulo;
						if (cierresXArt.ContainsKey(idArt))
							cierresXArt[idArt]--;
					}
					foreach (KeyValuePair<uint, int> kvp in cierresXArt)
					{
						if (kvp.Value < 0)//si le faltaron del mismo art está mal
						{
							Articulo art = Articulo.GetSingleE(kvp.Key);
							return new ABMResultado(enErrores.LogicaInvalida, "Al parecer estaba intentando hacer un cierre parcial sobre los surtidores de " + art.Codigo + " pero le faltaron " + Math.Abs(kvp.Value) + " surtidores por cerrar." +
								Environment.NewLine + "Verifique e intente nuevamente.");
						}
					}
					//emitir dif para parciales
					abmRes = FactEncabezado.EmitirDiferencia(out difs, cierres);
					if (abmRes.CodigoError != enErrores.Ninguno)
						return abmRes;
					//ver vales de la entrega parcial
					foreach (ValeEntrega vale in vales)
					{
						vale.Turno = turno.Numero + 1;//todavia no cierra el siguiente al ultimo
						vale.Fecha = parcial.Fecha;
						vale.IdParcial = parcial.IdParcial;
						abmRes = vale.Alta();
						if (abmRes.CodigoError != enErrores.Ninguno)
							return abmRes;
					}
					//si era un parcial basta con retornar aca, lo q sigue es para los totales nada mas, xq cierra el turno
					exito = true;
					return new ABMResultado(0);
				}
				else
				{
					//emitir dif para totales
					abmRes = FactEncabezado.EmitirDiferencia(out difs);
					if (abmRes.CodigoError != enErrores.Ninguno)
						return abmRes;//si se cerro todo, hay que cambiar de turno
					turno.Numero += 1;
					turno.FechaFinal = Reglas.Now;
					turno.FechaFinal = turno.FechaFinal.AddSeconds(-turno.FechaFinal.Second).AddMinutes(1);
					turno.IdVendedor = Reglas.VendedorActual.IdVendedor;
					turno.Propina = propina;
					//
					ABMResultado res = turno.Alta();
					if (res.CodigoError != enErrores.Ninguno)
						return res;
					foreach (ValeEntrega vale in vales)
					{
						vale.Turno = turno.Numero;//aca no es +1 porque se cambio al hacer el cierre
						vale.Fecha = turno.FechaFinal;
						vale.IdParcial = 0;
						res = vale.Alta();
						if (res.CodigoError != enErrores.Ninguno)
							return res;
					}
					exito = true;
					return res;
				}
			}
			catch (Exception ex)
			{
				difs = new FactEncabezado[0];
				Reglas.AddLog(ex);
				return new ABMResultado(enErrores.Otro, "Ocurrio un error inesperado");
			}
			finally
			{
				if (empeceTransa)
					Datos.FinTransa(exito);
			}
		}
		#endregion

	}
}