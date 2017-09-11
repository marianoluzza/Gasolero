using System;
using System.Collections.Generic;
using System.Text;

namespace X.Listados
{
	/// <summary>
	/// Esquemas de listado
	/// </summary>
	public class ListadoEsquema
	{
		string _dato;
		string _alias;
		int _pos = 0;
		string _alineacion = "I";

		/// <summary>
		/// Nombre del dato
		/// </summary>
		public string Dato
		{
			get { return _dato; }
			set { _dato = value; }
		}	

		/// <summary>
		/// Alias del dato, si corresponde
		/// </summary>
		public string Alias
		{
			get { return _alias; }
			set { _alias = value; }
		}

		/// <summary>
		/// Posicion relativa, mandatoria e irrepetible
		/// </summary>
		public int Posicion
		{
			get { return _pos; }
			set { _pos = value; }
		}

		/// <summary>
		/// (I)zquierda, (D)erecha o (C)entro
		/// </summary>
		public string Alineacion
		{
			get { return _alineacion; }
			set
			{
				if (value == null || value.Length == 0)
					return;
				_alineacion = value[0].ToString().ToUpper(); 
			}
		}
	}
}
