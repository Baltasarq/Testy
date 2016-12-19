using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Testy.Core {
	
	/// <summary>
	/// Represents a multiple-choice question.
	/// Text property is the question itself.
	/// Answer property returns all answers as a primitive vector of strings
	/// There are methods like AddAnswer(), GetAnswer(), ClearAnswers() and RemoveAnswer()
	/// for managing answers.
	/// </summary>
	public class Question
	{
		public const int MaxAnswers = 26;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Testy.Core.Question"/> class, with no answers.
		/// </summary>
		/// <param name='txt'>
		/// The text of the question itself.
		/// </param>
		public Question(string txt)
			: this()
		{
			this.Text = txt;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Testy.Core.Question"/> class,
		/// with an empty question and no answers.
		/// </summary>
		public Question()
		{
			this.Text = "Q?";
			this.answers = new List<string>();
			this.AddDefaultAnswer();
		}
		
		/// <summary>
		/// Adds a new answer.
		/// </summary>
		/// <param name='answer'>
		/// A string containing the answer
		/// </param>
		public void AddAnswer(string answer)
		{
			if ( this.CountAnswers >= MaxAnswers ) {
				throw new Exception( "answers above limit of: " + MaxAnswers );
			}
			
			this.answers.Add( answer.Trim() );
		}
		
		/// <summary>
		/// Inserts an answer in the given position.
		/// </summary>
		/// <param name='index'>
		/// The index in which to insert.
		/// </param>
		/// <param name='answer'>
		/// The answer itself, as a string.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Throws an exception when the index is outside limits of the collection of answers.
		/// </exception>
		public void InsertAnswer(int index, string answer)
		{
			if ( index >= 0
			  && index < this.answers.Count )
			{
				this.answers.Insert( index, answer.Trim() );
			} else {
				throw CreateInvalidArgumentIndexException( index );
			}
		}
		
		/// <summary>
		/// Removes the answer #index.
		/// </summary>
		/// <param name='index'>
		/// The index of the answer to remove.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Throws an exception when the index is outside limits of the collection of answers.
		/// </exception>
		public void RemoveAnswer(int index)
		{
			if ( index >= 0
			  && index < this.answers.Count )
			{
				this.answers.RemoveAt( index );
				
				if ( this.CountAnswers == 0 ) {
					this.AddDefaultAnswer();
				}
			} else {
				throw CreateInvalidArgumentIndexException( index );
			}
		}
		
		/// <summary>
		/// Adds the default answer (so there is at least one answer).
		/// </summary>
		protected void AddDefaultAnswer()
		{
			this.AddAnswer( "A: 1" );
		}
		
		/// <summary>
		/// Modifies the answer at index with the given value.
		/// </summary>
		/// <param name='index'>
		/// The index of the answer to modify.
		/// </param>
		/// <param name='value'>
		/// The value to substitute that answer with.
		/// </param>
		public void ModifyAnswerAt(int index, string value)
		{
			if ( index >= 0
			  && index < this.answers.Count )
			{
				this.answers[ index ] = value.Trim();
			} else {
				throw CreateInvalidArgumentIndexException( index );
			}
		}
		
		/// <summary>
		/// Gets the answer #index.
		/// </summary>
		/// <returns>
		/// The answer as a string
		/// </returns>
		/// <param name='index'>
		/// The index for the answer to retrieve
		/// </param>
		/// <exception cref="ArgumentException">
		/// Throws an exception when the index is outside limits of the collection of answers.
		/// </exception>
		public string GetAnswer(int index)
		{
			string toret;

			if ( index >= 0
			  && index < this.answers.Count )
			{
				toret = this.answers[ index ];
			} else {
				throw CreateInvalidArgumentIndexException( index );
			}

			return toret;
		}

		/// <summary>
		/// Shuffles the answers
		/// </summary>
		public void Shuffle() {
			int target = this.answers.Count;
			var shuffledAnswers = new List<string>( target );
			bool determinedCorrect = false;

			// Create a shuffled answers list
			foreach (int n in new RandomSequence( target ).Sequence) {
				shuffledAnswers.Add( this.answers[ n ] );

				// Store the correct answer in its new position
				if ( !determinedCorrect
				  && n == this.CorrectAnswer )
				{
					this.CorrectAnswer = shuffledAnswers.Count - 1;
					determinedCorrect = true;
				}
			}

			this.answers = shuffledAnswers;
			return;
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Testy.Core.Question"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Testy.Core.Question"/>.
		/// </returns>
		public override string ToString()
        {
            var toret = new StringBuilder();
			
            // Add the question's text
            toret.AppendLine( this.Text );
			
            // Add all the answers
			for(int i = 0; i < this.CountAnswers; ++i) {

                toret.Append( '\t' );
                toret.Append( (char) ( 'a' + i ) );
                toret.Append( ") " );
				toret.AppendLine( this.GetAnswer( i ) );
			}
			
			return toret.ToString();
		}

        /// <summary>
        /// Generates a html version.
        /// </summary>
        /// <returns>
        /// The html code in a string.
        /// </returns>
        public string ToHtml()
        {
            var toret = new StringBuilder();
            
            // Add the question's text
            toret.Append( "<li>" );
            toret.AppendLine( this.Text );
            
            // Add all the answers
            for(int i = 0; i < this.CountAnswers; ++i) {
                toret.Append( "<p>&nbsp;&nbsp;&nbsp;&nbsp;" );
                toret.Append( (char) ( 'a' + i ) );
                toret.Append( ") " );
                toret.Append( this.GetAnswer( i ) );
                toret.AppendLine( "</p>" );
            }

            toret.Append( "</li>" );
            toret.AppendLine( "<p/>" );
            return toret.ToString();
        }
		
		/// <summary>
		/// Creates the invalid argument index exception.
		/// </summary>
		/// <returns>
		/// The invalid argument index exception.
		/// </returns>
		/// <param name='index'>
		/// Index.
		/// </param>
		protected ArgumentException CreateInvalidArgumentIndexException(int index)
		{
			return new ArgumentException(
				"invalid index " + index
				+ "in answers for question: '" + this.Text + '\''
			);
		}
		
		/// <summary>
		/// Removes all answers.
		/// </summary>
		public void ClearAnswers()
		{
			this.answers.Clear();
		}
		
		/// <summary>
		/// Gets the answers as a read-only collection.
		/// </summary>
		/// <value>
		/// The answers as a ReadOnlyCollection instance.
		/// </value>
		public ReadOnlyCollection<string> Answers
		{
			get {
				return new ReadOnlyCollection<string>( this.answers.ToArray() );
			}
		}
		
		/// <summary>
		/// Returns the count of all answers.
		/// </summary>
		/// <returns>
		/// The number of answers, as an int.
		/// </returns>
		public int CountAnswers
		{
			get {
				return this.answers.Count;
			}
		}
		
		/// <summary>
		/// Gets or sets the text of the question.
		/// </summary>
		/// <value>
		/// The text, as a string.
		/// </value>
		public string Text {
			get {
				return this.text;
			}
			set {
				this.text = value.Trim();
			}
		}
		
		/// <summary>
		/// Gets or sets the correct answer index.
		/// </summary>
		/// <value>
		/// The correct answer, as an integet from 0 to maximum available answers.
		/// </value>
		/// <exception cref="ArgumentException">
		/// Throws an exception when the index is outside limits of the collection of answers.
		/// </exception>
		public int CorrectAnswer {
			get {
				return this.correctAnswer;
			}
			set {
				if ( value < 0
				  || value >= this.answers.Count )
				{
					throw CreateInvalidArgumentIndexException( value );
				}
				
				this.correctAnswer = value;
			}
		}
					
		private int correctAnswer;
		private string text;
		private List<string> answers;

	}
}
