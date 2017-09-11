using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio
{
	public enum enTipoMargenViolado
	{
		Ninguno,
		InfMenor,
		InfMayor,
		SupMenor,
		SupMayor,
		MaxMargenMayor,
		Cero
	}
}
