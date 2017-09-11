using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MarUtils.Soporte;

namespace X.Listados
{
	/// <summary>
	/// Binding entre datos y ListView
	/// </summary>
	public class ListadoBinding
	{
		ListView _lista;
		IList<ListadoEsquema> _esquemas = new List<ListadoEsquema>();
		SortedList<string, ListadoEsquema> _esquemasXDato;//dicc del anterior por dato
		

		IList<ListadoGrupo> _grupos = new List<ListadoGrupo>();//los grupos
		IAgrupador _agrupador = new AgrupadorSimple();

		DataSet _info = new DataSet();
		string _tablaMain;//q tabla es la q hay q mostrar
		string _datoGrupo;//q dato es el de los grupos

		//para totalizar, es decir, el ult item del grupo
		ListadoItemFinal _datosTotalizadores;
		//el grp resumen o total de totales
		ListadoGrupo _grupoTotalizador = new ListadoGrupo();
		//aca los datos del grp total de totales, si se necesitan
		//los @totalizadores, se suman
		//columna, nroFila, valor
		Matriz<string, int, string> _grupoTtTtDatos = new Matriz<string, int, string>();
		//los valores totalizados del grp total, {dato,total}
		SortedList<string, decimal> _grupoTtValoresTotalizados = new SortedList<string, decimal>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lista"></param>
		/// <param name="esquemas"></param>
		public ListadoBinding(ListView lista, IList<ListadoEsquema> esquemas)
		{
			_lista = lista;
			_esquemasXDato = new SortedList<string, ListadoEsquema>(esquemas.Count);
			SortedList<int, ListadoEsquema> esqOrdenados = new SortedList<int,ListadoEsquema>(esquemas.Count);
			foreach (ListadoEsquema esq in esquemas)
			{
				_esquemasXDato.Add(esq.Dato, esq);
				esqOrdenados.Add(esq.Posicion, esq);
				_grupoTtTtDatos.AddColumna(esq.Dato);
			}
			foreach (ListadoEsquema esq in esqOrdenados.Values)
				_esquemas.Add(esq);
			_datosTotalizadores = new ListadoItemFinal(esquemas);
		}

		/// <summary>
		/// Actualiza una tabla en el listado
		/// </summary>
		/// <param name="dt"></param>
		public void ActualizarTabla(DataTable dt)
		{
			if (!_info.Tables.Contains(dt.TableName))
			{
				if (dt.DataSet != null)
					_info.Tables.Add(dt.Copy());
				else
					_info.Tables.Add(dt);
			}
			else
			{
				_info.Tables[dt.TableName].Clear();
				_info.Tables[dt.TableName].AcceptChanges();
				_info.Tables[dt.TableName].Merge(dt);
				_info.Tables[dt.TableName].AcceptChanges();
			}
		}

		/// <summary>
		/// Obtiene o establece la tabla principal
		/// </summary>
		public string TablaMain
		{
			get { return _tablaMain; }
			set { _tablaMain = value; }
		}

		/// <summary>
		/// Obtiene o establece el dato con el que se agrupa
		/// </summary>
		public string DatoGrupo
		{
			get { return _datoGrupo; }
			set { _datoGrupo = value; }
		}

		/// <summary>
		/// Obtiene los datos totalizadores
		/// </summary>
		public ListadoItemFinal DatosTotalizadores
		{
			get { return _datosTotalizadores; }
		}

		/// <summary>
		/// Grupo totalizador o resumen, dejar el nombre en blanco para que no se use
		/// </summary>
		public ListadoGrupo GrupoTotalizador
		{
			get { return _grupoTotalizador; }
		}

		/// <summary>
		/// Listado de grupos
		/// </summary>
		public IList<ListadoGrupo> Grupos
		{
			get { return _grupos; }
			set { _grupos = value; }
		}

		/// <summary>
		/// Los datos que se usan el grupo resumen
		/// </summary>
		public Matriz<string, int, string> GrupoTotalizadorDatos
		{
			get { return _grupoTtTtDatos; }
		}

		/// <summary>
		/// Resetea la ligadura, usese la primera vez y cada vez que se cambian los datos o el formato
		/// </summary>
		public void ResetBinding()
		{
			_lista.Items.Clear();
			_lista.Groups.Clear();
			_lista.Columns.Clear();
			//la matriz tiene el total (decimal),
			//para cada dato (string fila) y
			//en cada grupo (string columna)
			Matriz<string, string, decimal> matriz = new Matriz<string, string, decimal>();
			//
			if (_esquemas.Count == 0)
				return;
			foreach (ListadoEsquema esq in _esquemas)
			{//agregado de columnas, una por esq
				ColumnHeader col = _lista.Columns.Add(String.IsNullOrEmpty(esq.Alias) ? esq.Dato : esq.Alias);
				switch (esq.Alineacion)
				{ 
					case "C":
						col.TextAlign = HorizontalAlignment.Center;
						break;
					case "D":
						col.TextAlign = HorizontalAlignment.Right;
						break;
					default:
						col.TextAlign = HorizontalAlignment.Left;
						break;
				}				
			}
			foreach (ListadoGrupo grp in _grupos)
			{//los grupos
				_lista.Groups.Add(grp.Clave, grp.Nombre).Tag = grp;
				matriz.AddColumna(grp.Clave);//cada grupo una columna
			}
			foreach (string s in _datosTotalizadores)
			{//lo q hay q totalizar en 
				if (s.StartsWith("@"))
					matriz.AddFila(s);//cada dato total es una fila
			}
			foreach (string col in _grupoTtTtDatos.GetCols())
			{//lo q hay q totalizar en el grp resumen o total de totales
				foreach (int fila in _grupoTtTtDatos.GetFilas())
					if (_grupoTtTtDatos[col, fila] != null && _grupoTtTtDatos[col, fila].StartsWith("@"))
						_grupoTtValoresTotalizados.Add(_grupoTtTtDatos[col, fila], 0);
			}
			//
			foreach (DataRow r in _info.Tables[TablaMain].Rows)
			{
				ListViewItem item = new ListViewItem();
				//a q grp va?
				string claveGrp = _agrupador.GetGrupo(r[DatoGrupo]);
				item.Text = r[_esquemas[0].Dato].ToString();
				for (int i = 1; i < _info.Tables[TablaMain].Columns.Count; i++)
				{
					string col = _info.Tables[TablaMain].Columns[i].ColumnName;
					//si la col esta entre los datos, lo agrego como subitem
					if (_esquemasXDato.ContainsKey(col))
						item.SubItems.Add(r[col].ToString());
					//si es uno de los q hay q totalizar, a la matriz
					if (_datosTotalizadores.ContieneValor("@" + col))					
						matriz[claveGrp, "@" + col] += decimal.Parse(r[col].ToString());
					//si pertenece al resumen, lo anoto
					if (_grupoTtValoresTotalizados.ContainsKey("@" + col))
						_grupoTtValoresTotalizados["@" + col] += decimal.Parse(r[col].ToString());
				}
				if (_lista.Groups.Count > 0 && !String.IsNullOrEmpty(DatoGrupo))
				{//si tiene grp se lo pongo
					item.Group = _lista.Groups[claveGrp];
					ListadoGrupo grp = item.Group.Tag as ListadoGrupo;
					if (grp.UsarColoresGrp)
					{
						item.ForeColor = grp.ForeColor;
						item.BackColor = grp.BackColor;
					}
				}
				_lista.Items.Add(item);
			}
			//listo los items!
			//si habia totalizadores los coloco
			if (!_datosTotalizadores.Vacio)
			{
				foreach (ListViewGroup grp in _lista.Groups)
				{//los reviso para cada grp
					ListViewItem item = new ListViewItem();
					string key = _datosTotalizadores[_esquemas[0]];
					item.Text = key.StartsWith("@") ? matriz[grp.Name, key].ToString() : key;
					for (int i = 1; i < _esquemas.Count; i++)
					{
						string s = _datosTotalizadores[_esquemas[i]];
						item.SubItems.Add(s.StartsWith("@") ? matriz[grp.Name, s].ToString() : s);
					}
					item.Group = grp;
					ListadoGrupo grpL = item.Group.Tag as ListadoGrupo;
					if (grpL.UsarColoresGrpTt)
					{
						item.ForeColor = grpL.ForeColorTt;
						item.BackColor = grpL.BackColorTt;
					}
					_lista.Items.Add(item);
				}
				//y ahora el resumen,
				//el total de totales, el pie, etc...
				if (_grupoTotalizador.Nombre != "")
				{
					_lista.Groups.Add(_grupoTotalizador.Clave, _grupoTotalizador.Nombre);
					foreach (int fila in _grupoTtTtDatos.GetFilas())
					{
						ListViewItem item = new ListViewItem();
						string valor = _grupoTtTtDatos[_esquemas[0].Dato, fila] ?? "";
						item.Text = valor.StartsWith("@") ? _grupoTtValoresTotalizados[valor].ToString() : valor;
						for (int i = 1; i < _esquemas.Count; i++)
						{
							valor = _grupoTtTtDatos[_esquemas[i].Dato, fila] ?? "";
							item.SubItems.Add(valor.StartsWith("@") ? _grupoTtValoresTotalizados[valor].ToString() : valor);
						}
						item.Group = _lista.Groups[_grupoTotalizador.Clave];
						if (_grupoTotalizador.UsarColoresGrp)
						{
							item.ForeColor = _grupoTotalizador.ForeColor;
							item.BackColor = _grupoTotalizador.BackColor;
						}
						_lista.Items.Add(item);
					}
				}
			}
			AutoResizeLista();
		}

		/// <summary>
		/// Reajusta la lista
		/// </summary>
		public void AutoResizeLista()
		{ 
			foreach(ColumnHeader col in _lista.Columns)
			{
				col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
				int w = col.Width;
				col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
				if (w > col.Width)
					col.Width = w;
			}
		}
	}
}
