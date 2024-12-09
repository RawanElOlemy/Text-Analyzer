module TextAnalyzer.Calculation

open System.Text.RegularExpressions
open System

// Function to count words (excluding punctuation)
let CountWord (text: string) =
    let regex = new Regex(@"\b[a-zA-Z0-9]+\b")
    let matches = regex.Matches(text)
    matches.Count

// Function to count sentences
let CountSentence (text: string) =
    let sentences = text.Split([|'.'; '!' ; '?'|], StringSplitOptions.RemoveEmptyEntries)
    Array.length sentences
