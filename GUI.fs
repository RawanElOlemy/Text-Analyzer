open System
open System.IO
open System.Drawing
open System.Windows.Forms
open TextAnalyzer.LoadTxt
open TextAnalyzer.Calculation

// Create the form
let form = new Form(Text = "Text Analyzer", Width = 600, Height = 600, StartPosition = FormStartPosition.CenterScreen)

// Create the main table layout
let tableLayoutPanel = new TableLayoutPanel(Dock = DockStyle.Fill)
tableLayoutPanel.RowCount <- 4
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 250.0f)) // TextBoxPanel height
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f)) // ListView takes remaining space
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60.0f)) // ButtonPanel height
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30.0f)) // ResultLabel height

// TextBox Panel
let textBoxPanel = new Panel(Dock = DockStyle.Fill, Padding = Padding(10))
let textBox = new TextBox(
    Multiline = true, 
    Dock = DockStyle.Fill, 
    ScrollBars = ScrollBars.Vertical, 
    BorderStyle = BorderStyle.FixedSingle, 
    Font = new Font("Consolas", 10.0f)
)
textBoxPanel.Controls.Add(textBox)
tableLayoutPanel.Controls.Add(textBoxPanel, 0, 0)

// ListView for word frequencies
let listView = new ListView(Dock = DockStyle.Fill, View = View.Details)
listView.Columns.Add("Word", 250, HorizontalAlignment.Left)  // Increased width for the "Word" column
listView.Columns.Add("Frequency", 100, HorizontalAlignment.Right)  // Width for the "Frequency" column
listView.Sorting <- SortOrder.Ascending
listView.Scrollable <- true
tableLayoutPanel.Controls.Add(listView, 0, 1)

// Button Panel with Load and Analyze buttons
let buttonPanel = new FlowLayoutPanel(
    Dock = DockStyle.Fill,
    FlowDirection = FlowDirection.LeftToRight,
    Padding = Padding(10),
    AutoSize = true
)

// Define the pink color
let pinkColor = Color.FromArgb(222, 49, 99)

// Create the Load button
let loadButton = TextAnalyzer.LoadTxt.createLoadButton()
loadButton.BackColor <- pinkColor  // Set pink background color
buttonPanel.Controls.Add(loadButton)

// Create the Analyze button
let analyzeButton = new Button(
    Text = "Analyze Text",
    Width = loadButton.Width,  // Ensure same width as Load button
    Height = loadButton.Height,  // Ensure same height as Load button
    FlatStyle = FlatStyle.Flat,
    BackColor = pinkColor,  // Set pink background color
    ForeColor = Color.White,
    Font = new Font("Segoe UI", 10.0f, FontStyle.Bold)
)
buttonPanel.Controls.Add(analyzeButton)

// Add the button panel to the table layout
tableLayoutPanel.Controls.Add(buttonPanel, 0, 2)

// Result Label for displaying analysis summary
let resultLabel = new Label(Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 10.0f))
tableLayoutPanel.Controls.Add(resultLabel, 0, 3)

// Add the main table layout to the form
form.Controls.Add(tableLayoutPanel)

// OpenFileDialog for file loading
let openFileDialog = new OpenFileDialog(Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*")

// Load button click event
loadButton.Click.Add(fun _ ->
    if openFileDialog.ShowDialog() = DialogResult.OK then
        try
            let fileContent = File.ReadAllText(openFileDialog.FileName)
            textBox.Text <- fileContent
        with
        | ex -> MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)

// Analyze button click event
let analyzeText () =
    try
        let fileContent = textBox.Text
        // Perform text analysis
        let wordCount = CountWord(fileContent)
        let sentenceCount = CountSentence(fileContent)
        let paragraphCount = CountParagraphs(fileContent)
        let wordFrequency = CalculateWordFrequency(fileContent) |> Seq.toList
        let mostFrequent = MostFrequentWords(fileContent) |> Seq.toList
        let averageSentenceLen = averageSentenceLength(fileContent)

        // Display results in the label
        let mostFrequentDisplay =
            mostFrequent
            |> List.map (fun (word, count) -> sprintf "%s: %d" word count)
            |> String.concat ", "
        
        resultLabel.Text <- sprintf "Words: %d | Sentences: %d | Paragraphs: %d | Avg Sentence Length: %d | Most Frequent Words: %s"
            wordCount sentenceCount paragraphCount averageSentenceLen mostFrequentDisplay

        // Populate ListView with word frequencies
        listView.Items.Clear()
        wordFrequency
        |> List.iter (fun (word, count) -> 
            let listItem = new ListViewItem(word)
            listItem.SubItems.Add(count.ToString()) |> ignore
            listView.Items.Add(listItem) |> ignore
        )
    with
    | ex -> MessageBox.Show($"Error analyzing text: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

analyzeButton.Click.Add(fun _ -> analyzeText())

// Run the form
[<STAThread>]
do Application.Run(form)
