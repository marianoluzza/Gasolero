using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Negocio
{
	public class FactItemCollection : IList<FactItem>, IList, ICollection, IEnumerable
	{
		List<FactItem> _items;
		FactEncabezado _encabezado;

		/// <summary>
		/// 
		/// </summary>
		public FactEncabezado Encabezado
		{
			get { return _encabezado; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enca"></param>
		public FactItemCollection(FactEncabezado enca)
		{
			_items = new List<FactItem>();
			_encabezado = enca;
		}

		#region IList<FactItem> Members
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(FactItem item)
		{
			return _items.IndexOf(item);
		}

		public void Insert(int index, FactItem item)
		{
			item.Coleccion = this;
			_items.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_items[index].Coleccion = null;
			_items.RemoveAt(index);
		}

		public FactItem this[int index]
		{
			get
			{
				return _items[index];
			}
			set
			{
				_items[index] = value;
				value.Coleccion = this;
			}
		}

		#endregion

		#region ICollection<FactItem> Members

		public void Add(FactItem item)
		{
			item.Coleccion = this;
			_items.Add(item);
		}

		public void Clear()
		{
			while (Count > 0)
				RemoveAt(0);
		}

		public bool Contains(FactItem item)
		{
			return _items.Contains(item);
		}

		public void CopyTo(FactItem[] array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _items.Count; }
		}

		public bool IsReadOnly
		{
			get { return ((ICollection<FactItem>)_items).IsReadOnly; }
		}

		public bool Remove(FactItem item)
		{
			return _items.Remove(item);
		}

		#endregion

		#region IEnumerable<FactItem> Members

		public IEnumerator<FactItem> GetEnumerator()
		{
			return ((IEnumerable<FactItem>)_items).GetEnumerator();
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
			FactItem item = value as FactItem;
			item.Coleccion = this;
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
			FactItem item = value as FactItem;
			item.Coleccion = this;
			((IList)_items).Insert(index, value);
		}

		public bool IsFixedSize
		{
			get { return ((IList)_items).IsFixedSize; }
		}

		public void Remove(object value)
		{
			FactItem item = value as FactItem;
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
				FactItem item = value as FactItem;
				item.Coleccion = this;
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
