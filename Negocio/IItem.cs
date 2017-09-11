using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio
{
	public interface IItem<TEnca>
	{
		/// <summary>
		/// Obtiene o establece la coleccion del item
		/// </summary>
		TEnca Coleccion
		{
			get;
			set;
		}
	}
}
