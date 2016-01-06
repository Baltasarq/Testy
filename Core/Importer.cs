﻿using System;
using System.IO;

namespace Testy.Core {
	/// <summary>
	/// Base class for all importers
	/// </summary>
	public abstract class Importer: Transformer, IDisposable {
		public Importer(string fileName)
			:base( fileName )
		{
			this.reader = new StreamReader( this.FileName );
		}

		/// <summary>
		/// Returns the next line from the input.
		/// A null return value indicates EOF.
		/// </summary>
		/// <returns>The line, as a string, or null.</returns>
		public string ReadLine() {
			return this.reader.ReadLine();
		}

		/// <summary>
		/// Loads a file given its name, in the specified format.
		/// </summary>
		/// <param name="fmt">The format the contents of the file are organized.</param>
		/// <param name="fileName">The file name, as a string.</param>
		public static Document Load(Transformer.Format fmt, string fileName) {
			Document toret;
			Importer importer = null;

			try {
				if ( fmt == Format.Text ) {
					importer = new TextImporter( fileName );
					toret = importer.Import();
				}
				else
				if ( fmt == Format.Xml ) {
					importer = new XmlImporter( fileName );
					toret = importer.Import();
				} else {
					throw new ArgumentException( "unrecognized format option" );
				}
			} catch(Exception) {
				throw;
			} finally {
				if ( importer != null ) {
					importer.Dispose();
				}
			}

			return toret;
		}

		/// <summary>
		/// Imports from input.
		/// </summary>
		public abstract Document Import();

		public void Dispose() {
			this.reader.Close();
		}

		private StreamReader reader;
	}
}

