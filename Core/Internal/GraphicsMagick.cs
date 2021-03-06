﻿using System.Diagnostics;
using System.IO;
using System;

namespace LtbDb.Core.Internal
{
	public static class GraphicsMagick
	{
		/// <summary>
		/// Path to GraphicsMagick binary. If null, "gm" will used.
		/// </summary>
		public static string Path { get; set; }

		/// <summary>
		/// Invoke "GraphicsMagick" via command line interface.
		/// </summary>
		/// <param name="source">The source image stream.</param>
		/// <param name="target">The target image stream.</param>
		/// <param name="arguments">GraphicsMagick arguments.</param>
		public static void PInvoke(Stream source, Stream target, string arguments)
		{
			using (var process = new Process())
			{
				process.StartInfo = new ProcessStartInfo
				{
					FileName = Path ?? "gm",
					Arguments = arguments,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				process.Start();

				source.CopyTo(process.StandardInput.BaseStream);
				process.StandardInput.Flush();
				process.StandardInput.Dispose();

				process.StandardOutput.BaseStream.CopyTo(target);

				var error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				if (process.ExitCode != 0)
				{
					throw new Exception(error);
				}
			}
		}
	}
}
