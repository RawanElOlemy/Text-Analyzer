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


// Function to count paragraphs
let CountParagraphs (text: string) =
    // Using regular expression to match one or more newlines
    let paragraphs = Regex.Split(text, @"\s*\n\s*\n\s*") |> Array.filter (fun s -> not (String.IsNullOrWhiteSpace(s)))
    paragraphs.Length

// Function to calculate word frequency
let CalculateWordFrequency (text: string) =
    // Remove punctuation and normalize text
    let cleanText = Regex.Replace(text.ToLower(), @"[^\w\s]", "")
    let words = cleanText.Split([|' '; '\n'; '\t'|], StringSplitOptions.RemoveEmptyEntries)
    words
    |> Seq.groupBy id
    |> Seq.map (fun (word, occurrences) -> word, Seq.length occurrences)
    |> Seq.sortByDescending snd

// Function to find the most frequently used words
let MostFrequentWords (text: string) =
    let wordFrequencies = CalculateWordFrequency text
    let maxFrequency = wordFrequencies |> Seq.map snd |> Seq.max
    wordFrequencies |> Seq.filter (fun (_, count) -> count = maxFrequency)
