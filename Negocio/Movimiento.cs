using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
	public class Movimiento
	{
		public uint IdMovimiento { get; set; }
		public uint IdVendedor { get; set; }
		public uint IdEntra { get; set; }
		public uint IdSale { get; set; }
		public DateTime Fecha { get; set; }
		public decimal Monto { get; set; }
		public uint IdTipoMovimiento { get; set; }
	}
}
