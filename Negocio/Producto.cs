using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MarUtils.Entidades;
using MarUtils.Soporte;
using DAL;

namespace Negocio
{
	public class Producto
	{

		public static DataTable GetAll()
		{
			return DatosMdb.ProductosGetAll();
		}
	}
}
