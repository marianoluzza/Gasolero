using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Negocio
{
	public class CajaEntregaItemCollection : CuerpoItemCollection<CajaEntrega, CajaEntregaItem>
	{
		public CajaEntregaItemCollection(CajaEntrega enca)
			: base(enca)
		{
		}
	}
}
