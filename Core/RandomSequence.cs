// Testy (c) 2017-2025 Baltasar MIT License <baltasarq@gmail.com>


namespace Testy.Core;


using System.Collections.ObjectModel;


/// <summary>
/// Random sequence generator.
/// It generates a list of unique integer numbers from 0 to Target - 1.
/// </summary>
public class RandomSequence {
	public RandomSequence(int target)
	{
		this.rnd = new Random( (int) DateTime.Now.TimeOfDay.TotalSeconds );
		this.Target = target;
		this.sequence = [];
		this.Generate();
	}

	/// <summary>
	/// Generates the sequence of integer numbers.
	/// </summary>
	private void Generate()
	{
		var seq = new HashSet<int>();

		while ( seq.Count < this.Target ) {
			seq.Add( this.rnd.Next( this.Target ) );
		}

		this.sequence.AddRange( seq );
	}

	/// <summary>
	/// Gets the sequence.
	/// </summary>
	/// <value>The sequence, as an int[].</value>
	public ReadOnlyCollection<int> Sequence {
		get => this.sequence.AsReadOnly<int>();
	}

	/// <summary>
	/// Gets the target (how long the sequence will be).
	/// </summary>
	/// <value>The target.</value>
	public int Target {
		get; private set;
	}

	private readonly Random rnd;
	private readonly List<int> sequence;
}
