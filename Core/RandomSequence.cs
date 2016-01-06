using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Testy.Core {
	/// <summary>
	/// Random sequence generator.
	/// It generates a list of unique integer numbers from 0 to Target - 1.
	/// </summary>
	public class RandomSequence {
		public RandomSequence(int target)
		{
			this.rnd = new Random( (int) DateTime.Now.TimeOfDay.TotalSeconds );
			this.Target = target;
			this.Generate();
		}

		/// <summary>
		/// Generates the sequence of integer numbers.
		/// </summary>
		private void Generate() {
			var seq = new HashSet<int>();
			var array = new int[ this.Target ];

			while ( seq.Count < this.Target ) {
				seq.Add( this.rnd.Next( this.Target ) );
			}

			seq.CopyTo( array );
			this.sequence = new ReadOnlyCollection<int>( array );
		}

		/// <summary>
		/// Gets the sequence.
		/// </summary>
		/// <value>The sequence, as an int[].</value>
		public ReadOnlyCollection<int> Sequence {
			get {
				return this.sequence;
			}
		}

		/// <summary>
		/// Gets the target (how long the sequence will be).
		/// </summary>
		/// <value>The target.</value>
		public int Target {
			get; private set;
		}

		private Random rnd;
		private ReadOnlyCollection<int> sequence;
	}
}

