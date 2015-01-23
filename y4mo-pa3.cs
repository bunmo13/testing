// -------------------------------------------------------------------	
// Department of Electrical and Computer Engineering
// University of Waterloo
//
// Student Name:     <Yifan Mo>
// Userid:           <y4mo>
//
// Assignment:       <Programming Assignment 3>
// Submission Date:  <November 14, 2014>
// 
// I declare that, other than the acknowledgements listed below, 
// this program is my original work.
//
// Acknowledgements:
// <N/A>
// ------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Class holds the sequence description of a protein.
class Protein
{
    string id;
    string name;
    string sequence;
    
    public Protein( string id, string name, string sequence )
    {
        this.id = id;
        this.name = name;
        this.sequence = sequence;
    }
    
    public string Id { get { return id; } }
    public string Name { get { return name; } }

    
	// The bool would return true for the protein sequence that
	// Contains the subsequence, and false otherwise. The 
	// string.Contain( ) method is used to determine whether 
	// the sequence contains the subsequence.
    public bool ContainsSubsequence( string sub )
    {
		if( sequence.Contains( sub ) )
		{
		  return true;
		}
		else
		{
          return false;
		}
    }
    
	// A new string newSequence is made to hold the original sequence
	// without the subsequence. The number of times that a subsequence 
	// occurs in the sequence is determined by the difference of the 
    // original length and the length of the new string divided by the 
    // length of the subsequence that passed as a parameter.	
    public int CountSubsequence( string sub )
    {
		int countResult = 0;
		string newSequence = sequence;
		
		while( newSequence.Contains( sub ) )
		{
		   newSequence= newSequence.Replace(  sub , "" );
		}
		
		countResult = ( sequence.Length - newSequence.Length ) / sub.Length;
		
        return countResult;
    }
    
    // A new string newSequence is made to show the location of the 
	// subsequence by matching the characters in the original sequence
    // with the characters in the subsequence. If a part of the sequence
    // matches the whole subsequence, the subsequence will be added to the
    // end of newSequence, otherwise "." is added to the end of newSequence. 	
    public string LocateSubsequence( string sub )
    {
		string newSequence = null;
		
		for( int i = 0; i < sequence.Length; i ++ )
		{	
		
		   if( sequence[ i ] == sub[ 0 ] )
		   {
		      for( int j = 1; j < sub.Length; j ++ )
		      {
		          if( sub[ j ] != sequence[ i + j ] )
			      {
			         newSequence =  newSequence + "..";
				     break;
			      }
			      if( j == sub.Length - 1 )
			      {   
			         i = i + sub.Length - 1;
				     newSequence = newSequence + sub;
					 break;
			      }
		      }
			}
			else
			{   
			   newSequence =  newSequence + ".";
			   
			}
		}
		return newSequence;
        
    }
    
    // Write the FASTA description of the protein to a given text stream.
	// The whole header and sequence of the protein will be displayed. The 
    // Console will start a new line if 80 characters limit is reached.	
    public void WriteFasta( TextWriter output )
    {
		
		Console.WriteLine( ">" + id + " " + name );
		
		for( int index = 0; index < sequence.Length; index ++ )
		{
		   
		   Console.Write( sequence[ index ] );
		   if( ( index + 1 ) % 80 == 0 )
		       Console.WriteLine( "" );     
		}
		Console.WriteLine( "" );
        
    }
}

// Read a protein file into a collection and test the functionality of
// methods in the Protein class.
static class Program
{
    static string fastaFile = "protein.fasta";
    
    
    static ArrayList proteins = new ArrayList( );
    
    
    static void Main( )
    {
        // Read proteins in FASTA format from the file.
        using( StreamReader sr = new StreamReader( fastaFile ) )
        {
            string line = sr.ReadLine( );
			
			 
            while( line != null )
            {
               
				// Find the header for the protein. If header cannot be 
				// found, an exception error message will display.
				string identifier = null;
				string name = null;
				string header = null;
				
				while( header == null && line != null  )
				{
				   if( line == "" )  
				   {
				     line = sr.ReadLine( ); 
					 
				   }
				   else if( line.StartsWith( ">" ) )
				   {
					    header = line;
					    line = sr.ReadLine( );
						
				   }
				   else 
				     throw new Exception( "expected a header line" );
				
				}
				
				// Find the sequence of the protein. If sequence cannot
				// be found, an exception message will display.
				string sequence = null;
				bool isComplete = false;
				
				while( isComplete == false && line != null )
				{
				   if( line == "" )
				     line = sr.ReadLine( );
				   else if( !line.StartsWith( ">" ) )
				   {
					    sequence += line;
                        line = sr.ReadLine( );
                   }						
				   else if( line.StartsWith( ">" ) )
				   {
				       if( sequence != null )
					        isComplete = true;
				   }
				   else
				      throw new Exception( "expected a sequence" );
				}
				
				
				// Find the id and name of the protein by breaking the 
				// header into two parts. And then add the protein into
				// the collection ArrayList. Error message will displayed
				// if the information is missing.
				if( header != null )
				{
				   if( sequence != null )
				    {  
					
					    string[] splitLine = header.Split( '>' );
					    identifier = splitLine[1].Remove( 10 );
						name = splitLine[1].Remove( 0, 11 );
						
						
					   Protein someProtein 
					       = new Protein( identifier, name, sequence );
					   proteins.Add( someProtein );
					}
				   else 
				      throw new Exception( "header with missing sequence" );
				}
            }
        }
        
        // Report count of proteins loaded.
        Console.WriteLine( );
        Console.WriteLine( "Loaded {0} proteins from the {1} file.", 
            proteins.Count, fastaFile );
          
        // Report proteins containing a specified sequence.
        Console.WriteLine( );
        Console.WriteLine( "Proteins containing sequence RILED:" );
        foreach( Protein p in proteins )
        {
            if( p.ContainsSubsequence( "RILED" ) )
            {
                Console.WriteLine( p.Name );
            }
        }
        
        // Report proteins containing a repeated sequence.
        Console.WriteLine( );
        Console.WriteLine( 
            "Proteins containing sequence SNL more than 5 times:" );
        foreach( Protein p in proteins )
        {
            if( p.CountSubsequence( "SNL" ) > 5 )
            {
                Console.WriteLine( p.Name );
            }
        }
        
        // Locate the specified sequence in proteins containing it.
        Console.WriteLine( );
        Console.WriteLine( "Proteins containing sequence DEVGG:" );
        foreach( Protein p in proteins )
        {
            if( p.ContainsSubsequence( "DEVGG" ) )
            {
                Console.WriteLine( p.Name );
                Console.WriteLine( p.LocateSubsequence( "DEVGG" ) );
            }
        }
        
        // Show FASTA output for proteins containing a specified sequence.
        Console.WriteLine( );
        Console.WriteLine( "Proteins containing sequence DEVGG:" );
        foreach( Protein p in proteins )
        {
            if( p.ContainsSubsequence( "DEVGG" ) )
            {
                p.WriteFasta( Console.Out );
            }
        }
        
    }
}