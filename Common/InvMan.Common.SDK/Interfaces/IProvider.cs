using System;

namespace InvMan.Common.SDK
{
	public interface IProvider
	{
        /// <summary>
        /// Host to address to. Default is http://localhost:5000
        /// </summary>
		Uri Host { get; set; }
	}
}
