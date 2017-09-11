using System;
using System.Collections.Generic;
using System.Text;

namespace X.Listados
{
	/// <summary>
	/// Interfaz de agrupacion
	/// </summary>
	public interface IAgrupador
	{
		/// <summary>
		/// Devuelve la clave de grupo al que pertenece el objeto
		/// </summary>
		string GetGrupo(object obj);
	}

	/// <summary>
	/// Agrupador simple, agrupa por ToString
	/// </summary>
	public class AgrupadorSimple : IAgrupador
	{
		/// <summary>
		/// Obtiene el grupo del objeto
		/// </summary>
		public virtual string GetGrupo(object obj)
		{
			return obj.ToString();
		}
	}

	/// <summary>
	/// Agrupador por letra inicial
	/// </summary>
	public class AgrupadorIniciales : IAgrupador
	{
		int _cantIniciales;

		/// <summary>
		/// Crea una instancia del agrupador
		/// </summary>
		/// <param name="cantIniciales"></param>
		public AgrupadorIniciales(int cantIniciales)
		{
			_cantIniciales = cantIniciales;
		}

		/// <summary>
		/// Obtiene el grupo del objeto
		/// </summary>
		public virtual string GetGrupo(object obj)
		{
			string clave = obj.ToString();
			return clave.Length < _cantIniciales ? clave : clave.Substring(0, _cantIniciales);
		}
	}

	/// <summary>
	/// Agrupador por mes
	/// </summary>
	public class AgrupadorMeses : IAgrupador
	{
		/// <summary>
		/// Obtiene el grupo del objeto
		/// </summary>
		public virtual string GetGrupo(object obj)
		{
			DateTime clave = DateTime.Parse(obj.ToString());
			switch (clave.Month)
			{ 
				case 1:
					return "Enero";
				case 2:
					return "Febrero";
				case 3:
					return "Marzo";
				case 4:
					return "Abril";
				case 5:
					return "Mayo";
				case 6:
					return "Junio";
				case 7:
					return "Julio";
				case 8:
					return "Agosto";
				case 9:
					return "Septiembre";
				case 10:
					return "Octubre";
				case 11:
					return "Noviembre";
				default:
					return "Diciembre";
			}
		}
	}
}
