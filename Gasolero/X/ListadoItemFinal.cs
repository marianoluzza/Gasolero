using System;
using System.Collections.Generic;
using System.Text;

namespace X.Listados
{
	/// <summary>
	/// Representa el ultimo renglon del grupo
	/// </summary>
	public class ListadoItemFinal : IEnumerable<string>
	{
		Dictionary<ListadoEsquema, string> _datosTotalizadoresXEsq = new Dictionary<ListadoEsquema, string>();
		Dictionary<ListadoEsquema, bool> _vacios = new Dictionary<ListadoEsquema, bool>();
		SortedList<string, ListadoEsquema> _esquemasXDatoAlias = new SortedList<string, ListadoEsquema>();
		SortedList<string, string> _datosTotalizadoresValues = new SortedList<string, string>();

		/// <summary>
		/// Item final totalizador esquematizado
		/// </summary>
		/// <param name="esquemas">esquemas del listado</param>
		public ListadoItemFinal(IList<ListadoEsquema> esquemas)
		{
			foreach (ListadoEsquema esq in esquemas)
			{
				_datosTotalizadoresXEsq.Add(esq, "");
				_vacios.Add(esq, true);
				_esquemasXDatoAlias.Add(esq.Dato, esq);
				if (!String.IsNullOrEmpty(esq.Alias))
					_esquemasXDatoAlias.Add(esq.Alias, esq);
			}
		}

		/// <summary>
		/// Obtiene o establece el label total del esquema correspondiente
		/// </summary>
		/// <param name="esq">esquema</param>
		/// <returns></returns>
		public string this[ListadoEsquema esq]
		{
			get
			{
				return _datosTotalizadoresXEsq[esq];
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					if (!_vacios.ContainsKey(esq))
						_vacios.Add(esq, true);
				}
				else if (_vacios.ContainsKey(esq))
					_vacios.Remove(esq);
				//
				//seteo
				string valAnt = _datosTotalizadoresXEsq[esq];
				if (valAnt == value)
					return;
				_datosTotalizadoresXEsq[esq] = value;
				//
				//agregar el valor nuevo y quitar el viejo del indice de valores
				if (_datosTotalizadoresValues.ContainsKey(valAnt))
					_datosTotalizadoresValues.Remove(valAnt);
				_datosTotalizadoresValues.Add(value, value);
			}
		}

		/// <summary>
		/// Obtiene o establece el label total del esquema correspondiente
		/// </summary>
		/// <param name="dato">dato o alias del esquema</param>
		/// <returns></returns>
		public string this[string dato]
		{
			get { return this[_esquemasXDatoAlias[dato]]; }
			set { this[_esquemasXDatoAlias[dato]] = value; }
		}

		/// <summary>
		/// Dice si esta vacio el item final
		/// </summary>
		public bool Vacio
		{
			get { return _vacios.Count == _datosTotalizadoresXEsq.Count; }
		}

		/// <summary>
		/// Dice si contiene el valor dado
		/// </summary>
		/// <param name="val">valor de un label en el item totalizador</param>
		/// <returns></returns>
		public bool ContieneValor(string val)
		{
			return _datosTotalizadoresValues.ContainsKey(val);
		}

		#region IEnumerable<string> Members
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator<string> GetEnumerator()
		{
			SortedList<int, string> res = new SortedList<int, string>();
			foreach (KeyValuePair<ListadoEsquema, string> esq in _datosTotalizadoresXEsq)
			{
				res.Add(esq.Key.Posicion, esq.Value);
			}
			return res.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
