using System;
namespace asp_net_with_angular_client_template.Services.Interface
{
	public interface IHashService
	{
		/// <summary>
		/// Return hash from input string
		/// </summary>
		/// <param name="inputString"></param>
		/// <returns></returns>
		string GetHash(string inputString);
	}
}

