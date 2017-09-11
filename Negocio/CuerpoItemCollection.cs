using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Negocio
{
	public class CuerpoItemCollection<TEnca, TCpo> : IList<TCpo>, IList, ICollection, IEnumerable 
		where TEnca : class 
		where TCpo : class, IItem<TEnca>
	{
		List<TCpo> _items;
		TEnca _encabezado;

		/// <summary>
		/// 
		/// </summary>
		public TEnca Encabezado
		{
			get { return _encabezado; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enca"></param>
		public CuerpoItemCollection(TEnca enca)
		{
			_items = new List<TCpo>();
			_encabezado = enca;
		}

		#region IList<TCpo> Members
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(TCpo item)
		{
			return _items.IndexOf(item);
		}

		public void Insert(int index, TCpo item)
		{
			item.Coleccion = _encabezado;
			_items.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_items[index].Coleccion = default(TEnca);
			_items.RemoveAt(index);
		}

		public TCpo this[int index]
		{
			get
			{
				return _items[index];
			}
			set
			{
				_items[index] = value;
				value.Coleccion = _encabezado;
			}
		}

		#endregion

		#region ICollection<TCpo> Members

		public void Add(TCpo item)
		{
			item.Coleccion = _encabezado;
			_items.Add(item);
		}

		public void Clear()
		{
			while (Count > 0)
				RemoveAt(0);
		}

		public bool Contains(TCpo item)
		{
			return _items.Contains(item);
		}

		public void CopyTo(TCpo[] array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _items.Count; }
		}

		public bool IsReadOnly
		{
			get { return ((ICollection<TCpo>)_items).IsReadOnly; }
		}

		public bool Remove(TCpo item)
		{
			return _items.Remove(item);
		}

		#endregion

		#region IEnumerable<TCpo> Members

		public IEnumerator<TCpo> GetEnumerator()
		{
			return ((IEnumerable<TCpo>)_items).GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((System.Collections.IEnumerable)_items).GetEnumerator();
		}

		#endregion

		#region IList Members

		public int Add(object value)
		{
			TCpo item = value as TCpo;
			item.Coleccion = _encabezado;
			return ((IList)_items).Add(value);
		}

		public bool Contains(object value)
		{
			return ((IList)_items).Contains(value);
		}

		public int IndexOf(object value)
		{
			return ((IList)_items).IndexOf(value);
		}

		public void Insert(int index, object value)
		{
			TCpo item = value as TCpo;
			item.Coleccion = _encabezado;
			((IList)_items).Insert(index, value);
		}

		public bool IsFixedSize
		{
			get { return ((IList)_items).IsFixedSize; }
		}

		public void Remove(object value)
		{
			TCpo item = value as TCpo;
			item.Coleccion = null;
			((IList)_items).Remove(value);
		}

		object IList.this[int index]
		{
			get
			{
				return ((IList)_items)[index];
			}
			set
			{
				TCpo item = value as TCpo;
				item.Coleccion = _encabezado;
				((IList)_items)[index] = value;
			}
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			((IList)_items).CopyTo(array, index);
		}

		public bool IsSynchronized
		{
			get { return ((IList)_items).IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return ((IList)_items).SyncRoot; }
		}

		#endregion
	}
}
