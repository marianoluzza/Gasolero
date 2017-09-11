using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Cliente : EntidadNombrada
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Clientes"; } }
		public static string NombreEntidad
		{ get { return "Cliente"; } }
		public static string NombreClave
		{ get { return "IdCliente"; } }

		#endregion

		#region Atributos y Propiedades
		System.String _Direccion = "";
		public System.String Direccion
		{
			get { return _Direccion; }
			set { _Direccion = value; }
		}

		System.String _CUIT = "";
		public System.String CUIT
		{
			get { return _CUIT; }
			set { _CUIT = value; }
		}

		System.String _Telefono = "";
		public System.String Telefono
		{
			get { return _Telefono; }
			set { _Telefono = value; }
		}

		public System.UInt32 IdCliente
		{
			get { return Id; }
			set { Id = value; }
		}

		bool _respInscripto = false;
		public bool RespInscripto
		{
			get { return _respInscripto; }
			set { _respInscripto = value; }
		}

		public bool Exento { get; set; }

		static private Cliente _consFinal = null;
		/// <summary>
		/// Consumidor Final. Devuelve clon
		/// </summary>
		static public Cliente ConsFinal
		{
			get { return _consFinal.Clonar(); }
			internal set { _consFinal = value; }
		}

		public bool EsConsFinal
		{
			get { return Id == 1; }
		}

		public decimal SaldoAdvertencia { get; set; }
		public decimal SaldoMaximo { get; set; }

		public string Email { get; set; }

		/// <summary>
		/// En %
		/// </summary>
		public decimal Descuento { get; set; }
		#endregion

		#region Constructores
		public Cliente()
		{
			Id = 1;
		}

		public Cliente(DataRow r)
		{
			Nombre = r["Nombre"].ToString();
			Direccion = r["Direccion"].ToString();
			CUIT = r["CUIT"].ToString();
			Telefono = r["Telefono"].ToString();
			Id = uint.Parse(r["IdCliente"].ToString());
			RespInscripto = r["RespInscripto"].ToString() == "1" || r["RespInscripto"].ToString().ToUpper() == "SI" || r["RespInscripto"].ToString().ToUpper() == "TRUE";
			if (r.Table.Columns.Contains("Exento"))
				Exento = r["Exento"].ToString() == "1" || r["Exento"].ToString().ToUpper() == "SI" || r["Exento"].ToString().ToUpper() == "TRUE";
			else
				Exento = false;
			if (r.Table.Columns.Contains("SaldoAdvertencia"))
				SaldoAdvertencia = decimal.Parse(r["SaldoAdvertencia"].ToString());
			else
				SaldoAdvertencia = 0;
			if (r.Table.Columns.Contains("SaldoMaximo"))
				SaldoMaximo = decimal.Parse(r["SaldoMaximo"].ToString());
			else
				SaldoMaximo = 0;
			if (r.Table.Columns.Contains("Email"))
				Email = r["Email"].ToString();
			else
				Email = "";
			if (r.Table.Columns.Contains("Descuento"))
				Descuento = decimal.Parse(r["Descuento"].ToString());
			else
				Descuento = 0;
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Clientes);
		}

		public Cliente Clonar()
		{
			Cliente res = new Cliente();
			res._CUIT = _CUIT;
			res._Direccion = _Direccion;
			res._id = _id;
			res._nombre = _nombre;
			res._respInscripto = _respInscripto;
			res._Telefono = _Telefono;
			return res;
		}

		public bool CUITCheck()
		{
			string cuit = CUIT.Replace("-", "").Trim();
			try
			{
				int[] S = new int[11];
				int i = 0;
				foreach (char c in cuit)
					S[i++] = int.Parse(c.ToString());
				int v2 = (S[0] * 5 +
					S[1] * 4 +
					S[2] * 3 +
					S[3] * 2 +
					S[4] * 7 +
					S[5] * 6 +
					S[6] * 5 +
					S[7] * 4 +
					S[8] * 3 +
					S[9] * 2) % 11;
				int v3 = 11 - v2;
				switch (v3)
				{
					case 11:
						v3 = 0;
						break;
					case 10:
						v3 = 9;
						break;
				}
				return S[10] == v3;
			}
			catch
			{
				return false;
			}
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.Clientes);
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.Clientes, NombreClave, id);
		}

		public static Cliente GetSingleE(uint id)
		{
			return new Cliente(GetSingle(id).Rows[0]);
		}

		public static Cliente GetSingleE(string cuit)
		{
			DataTable dtCuitSimil = Datos.ClientesGetListForCuit(cuit);
			if (dtCuitSimil.Rows.Count > 0)
			{
				Cliente cCuit = new Cliente(dtCuitSimil.Rows[0]);
				return cCuit;
			}
			else
				return null;
		}

		public static DataTable GetListForNombre(string nbre)
		{
			return DAL.Datos.ClientesGetListForNombre(nbre);
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			ABMResultado res;
			string cuitLimpio = CUIT.Replace("-", "").Trim();
			if (cuitLimpio.Length > 0)
			{
				DataTable dtCuitSimil = Datos.ClientesGetListForCuit(CUIT);
				if (dtCuitSimil.Rows.Count > 0)
				{
					Cliente cCuit = new Cliente(dtCuitSimil.Rows[0]);
					res = new ABMResultado(enErrores.LogicaInvalida, "El CUIT ya existe, ingresado para " + cCuit.Nombre);
					return res;
				}
			}
			else if (RespInscripto || Exento)
			{
				res = new ABMResultado(enErrores.LogicaInvalida, "El CUIT es obligatorio para responsables inscriptos o exentos");
				return res;
			}
			//
			if (cuitLimpio.Length > 0)
			{
				if (!CUITCheck())
					return new ABMResultado(enErrores.LogicaInvalida, "El CUIT es inválido");
			}
			else
				CUIT = "";
			//
			if (RespInscripto && Exento)
				return new ABMResultado(enErrores.LogicaInvalida, "El cliente no puede ser responsable inscripto y exento a la vez");
			//
			if (Nombre.Trim() == "")
				return new ABMResultado(enErrores.LogicaInvalida, "Falta el nombre del cliente");
			else
			{
				DataTable dtSimilares = GetListForNombre(Nombre);
				if (dtSimilares.Rows.Count > 0)
				{
					return new ABMResultado(enErrores.YaExiste, "Ya existe un cliente con ese nombre.");
				}
			}
			if (Email != "")
			{
				bool IsValidEmail = false;
				try
				{
					var addr = new System.Net.Mail.MailAddress(Email);
					IsValidEmail = addr.Address == Email;
				}
				catch
				{
					IsValidEmail = false;
				}
				if (!IsValidEmail)
					return new ABMResultado(enErrores.FormatoIncorrecto, "El email es inválido");
			}
			res = DAL.Datos.ABM(DAL.enTablas.Clientes, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdCliente = res.IdInsercion;
			return res;
		}

		public override ABMResultado Modificacion()
		{
			if (Id == 0)
				return new ABMResultado(enErrores.LogicaInvalida, "No se puede modificar un cliente inexistente. Use ingresar. ");
			if (Id == 1)
				return new ABMResultado(enErrores.Cancelado, "No se puede modificar este cliente");
			if (RespInscripto && Exento)
				return new ABMResultado(enErrores.LogicaInvalida, "El cliente no puede ser responsable inscripto y exento a la vez");
			string cuitLimpio = CUIT.Replace("-", "").Trim();
			if (cuitLimpio.Length > 0)
			{
				DataTable dtCuitSimil = Datos.ClientesGetListForCuit(CUIT);
				if (dtCuitSimil.Rows.Count > 0)
				{
					Cliente cCuit = new Cliente(dtCuitSimil.Rows[0]);
					if (cCuit.Id != Id)
						return new ABMResultado(enErrores.LogicaInvalida, "El CUIT ya existe, ingresado para " + cCuit.Nombre);
				}
			}
			else if (RespInscripto || Exento)
			{
				return new ABMResultado(enErrores.LogicaInvalida, "El CUIT es obligatorio para responsables inscriptos o exentos");
			}
			//
			if (cuitLimpio.Length > 0)
			{
				if (!CUITCheck())
					return new ABMResultado(enErrores.LogicaInvalida, "El CUIT es inválido");
			}
			else
				CUIT = "";
			//
			if (Nombre.Trim() == "")
				return new ABMResultado(enErrores.LogicaInvalida, "Falta el nombre del cliente");
			else
			{
				DataTable dtSimilares = GetListForNombre(Nombre);
				foreach (DataRow r in dtSimilares.Rows)
				{
					Cliente cSimil = new Cliente(r);
					if (cSimil.Id != Id)
						return new ABMResultado(enErrores.YaExiste, "Ya existe un cliente con ese nombre.");
				}
			}
			if (Email != "")
			{
				bool IsValidEmail = false;
				try
				{
					var addr = new System.Net.Mail.MailAddress(Email);
					IsValidEmail = addr.Address == Email;
				}
				catch
				{
					IsValidEmail = false;
				}
				if (!IsValidEmail)
					return new ABMResultado(enErrores.FormatoIncorrecto, "El email es inválido");
			}
			return DAL.Datos.ABM(DAL.enTablas.Clientes, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			if (Id == 1)
				return new ABMResultado(enErrores.Cancelado, "No se puede eliminar este cliente");
			else if (Reglas.VendedorActual.EsRol(enPermisos.Vendedor) <= 0)
				return new ABMResultado(enErrores.Cancelado, "No puede eliminar clientes");
			return DAL.Datos.ABM(DAL.enTablas.Clientes, enABMFuncion.Baja, ProtocoloDatos());
		}

		#endregion

	}
}