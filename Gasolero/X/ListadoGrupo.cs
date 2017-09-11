using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace X.Listados
{
	/// <summary>
	/// Grupos de listado
	/// </summary>
	public class ListadoGrupo
	{
		string _clave;
		string _nombre;

		Color _foreColor = SystemColors.ControlText;
		Color _backColor = Color.White;
		bool _usarColoresGrp = false;

		Color _foreColorTt = SystemColors.ControlText;
		Color _backColorTt = Color.White;
		bool _usarColoresGrpTt = false;

		#region Constructores
		/// <summary>
		/// 
		/// </summary>
		public ListadoGrupo()
		{ 
		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nombre"></param>
		public ListadoGrupo(string nombre)
			: this(nombre, nombre)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nombre"></param>
		/// <param name="clave"></param>
		public ListadoGrupo(string nombre, string clave)
			: this()
		{
			_nombre = nombre;
			_clave = clave;
		}
		#endregion
		
		/// <summary>
		/// Obtiene o establece el nombre del grupo
		/// </summary>
		public string Nombre
		{
			get { return _nombre; }
			set { _nombre = value; }
		}

		/// <summary>
		/// Obtiene o establece el valor de la clave del grupo
		/// </summary>
		public string Clave
		{
			get { return _clave; }
			set { _clave = value; }
		}

		/// <summary>
		/// Color de fuente del item total
		/// </summary>
		public Color ForeColorTt
		{
			get { return _foreColorTt; }
			set { _foreColorTt = value; }
		}

		/// <summary>
		/// Color de fondo del item total
		/// </summary>
		public Color BackColorTt
		{
			get { return _backColorTt; }
			set { _backColorTt = value; }
		}

		/// <summary>
		/// Usar colores personalizados para el item total?
		/// </summary>
		public bool UsarColoresGrpTt
		{
			get { return _usarColoresGrpTt; }
			set { _usarColoresGrpTt = value; }
		}

		/// <summary>
		/// Color de fuente de los items del grupo
		/// </summary>
		public Color ForeColor
		{
			get { return _foreColor; }
			set { _foreColor = value; }
		}

		/// <summary>
		/// Color de fondo de los items del grupo
		/// </summary>
		public Color BackColor
		{
			get { return _backColor; }
			set { _backColor = value; }
		}

		/// <summary>
		/// Usar colores personalizados para los items del grupo?
		/// </summary>
		public bool UsarColoresGrp
		{
			get { return _usarColoresGrp; }
			set { _usarColoresGrp = value; }
		}
	}
}
