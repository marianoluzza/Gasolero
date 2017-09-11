using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public partial class Vendedor : EntidadBase
	{
		#region IDatos
		public static string NombreTabla
		{ get { return "Vendedores"; } }
		public static string NombreEntidad
		{ get { return "Vendedor"; } }
		public static string NombreClave
		{ get { return "IdVendedor"; } }

		#endregion

		#region Atributos y Propiedades
		System.UInt32 _IdVendedor = 0;
		public System.UInt32 IdVendedor
		{
			get { return _IdVendedor; }
			set { _IdVendedor = value; }
		}

		System.String _Nombre = "";
		public System.String Nombre
		{
			get { return _Nombre; }
			set { _Nombre = value.Trim(); }
		}

		string _Password = "";
		public System.String Password
		{
			get { return _Password; }
			set { _Password = value; }
		}

		System.Int32 _Permisos = 0;
		public System.Int32 Permisos
		{
			get { return _Permisos; }
			set { _Permisos = value; }
		}
		#endregion

		#region Constructores
		public Vendedor() { }

		public Vendedor(DataRow r)
		{
			IdVendedor = uint.Parse(r["IdVendedor"].ToString());
			Nombre = r["Nombre"].ToString();
			Password = r["Password"].ToString();
			Permisos = int.Parse(r["Permisos"].ToString());
		}

		#endregion

		#region Metodos
		public static DataTable Esquema()
		{
			return DAL.Datos.Esquema(enTablas.Vendedores);
		}

		/// <summary>
		/// Dice si es el rol dado
		/// </summary>
		/// <param name="rol">que rol</param>
		/// <returns>=0 es ese rol, >0 es mas que rol </returns>
		public int EsRol(enPermisos rol)
		{
			return Permisos - (int)rol;
		}

		/// <summary>
		/// Añade o resetea el permiso correspondiente
		/// </summary>
		/// <param name="permiso">que permiso</param>
		/// <param name="add">añadir o establecer?</param>
		public void SetPermiso(enPermisos permiso, bool add)
		{
			if (add)
				Permisos |= (int)permiso;
			else
				Permisos = (int)permiso;
		}

		/// <summary>
		/// Dice si el usuario tiene el permiso correspondiente
		/// </summary>
		public bool TienePermiso(enPermisos permiso)
		{
			return (Permisos & (int)permiso) == (int)permiso;
		}
		#endregion

		#region Consultas
		public static DataTable GetAll()
		{
			return DAL.Datos.GetAll(DAL.enTablas.Vendedores);
		}

		public static DataTable GetSingle(uint id)
		{
			return DAL.Datos.GetSingle(DAL.enTablas.Vendedores, NombreClave, id);
		}

		public static Vendedor GetSingleE(uint id)
		{
			return new Vendedor(GetSingle(id).Rows[0]);
		}

		public static DataTable GetListForNombre(string nbre)
		{
			return DAL.Datos.VendedoresGetListForNombre(nbre);
		}

		public ABMResultado CambiarPass(string nvoPass)
		{
			if (IdVendedor == 0)
				return new ABMResultado(enErrores.LogicaInvalida, "Primero termine el alta del vendedor y luego cambie la contraseña");
			Password = Crypto.HashU2Hexa(nvoPass);
			return Modificacion();
		}
		#endregion

		#region ABM
		public override ABMResultado Alta()
		{
			Password = "";
			DataTable dtSimilares = GetListForNombre(Nombre);
			if (dtSimilares.Rows.Count > 0)
			{
				return new ABMResultado(enErrores.YaExiste, "Ya existe un vendedor con ese nombre.");
			}
			ABMResultado res = DAL.Datos.ABM(DAL.enTablas.Vendedores, enABMFuncion.Alta, ProtocoloDatos());
			if (res.CodigoError == enErrores.Ninguno)
				IdVendedor = res.IdInsercion;
			return res;
		}

		public override ABMResultado Modificacion()
		{
			if (IdVendedor == 0)
				return new ABMResultado(enErrores.LogicaInvalida, "No se puede modificar un vendedor inexistente. Use ingresar. ");
			if (IdVendedor == 1 && Reglas.VendedorActual.IdVendedor != 1)
				return new ABMResultado(enErrores.Cancelado, "No se puede modificar este usuario");
			//
			if (Nombre.Trim() == "")
				return new ABMResultado(enErrores.LogicaInvalida, "Falta el nombre del vendedor");
			else
			{
				DataTable dtSimilares = GetListForNombre(Nombre);
				foreach (DataRow r in dtSimilares.Rows)
				{
					Vendedor vSimil = new Vendedor(r);
					if (vSimil.IdVendedor != IdVendedor)
						return new ABMResultado(enErrores.YaExiste, "Ya existe un vendedor con ese nombre.");
				}
			}
			return DAL.Datos.ABM(DAL.enTablas.Vendedores, enABMFuncion.Modificacion, ProtocoloDatos());
		}

		public override ABMResultado Baja()
		{
			if (IdVendedor == 1)
				return new ABMResultado(enErrores.Cancelado, "No se puede eliminar este usuario");
			if (Reglas.VendedorActual.EsRol((enPermisos)Permisos) <= 0)
				return new ABMResultado(enErrores.Cancelado, "No puede eliminar un usuario con mayor rango");
			return DAL.Datos.ABM(DAL.enTablas.Vendedores, enABMFuncion.Baja, ProtocoloDatos());
		}

		#endregion

	}
}