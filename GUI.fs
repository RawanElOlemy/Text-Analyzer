open System
open System.IO
open System.Drawing
open System.Windows.Forms
open TextAnalyzer.LoadTxt
open TextAnalyzer.Calculation

let form = new Form(Text = "Text Analyzer", Width = 600, Height = 600, StartPosition = FormStartPosition.CenterScreen)

let tableLayoutPanel = new TableLayoutPanel(Dock = DockStyle.Fill)
tableLayoutPanel.RowCount <- 4
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 250.0f)) // TextBoxPanel height
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f)) // ListView takes remaining space
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60.0f)) // ButtonPanel height
tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30.0f)) // ResultLabel height

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

let listView = new ListView(Dock = DockStyle.Fill, View = View.Details)
listView.Columns.Add("Word", 250, HorizontalAlignment.Left)  // Increased width for the "Word" column
listView.Columns.Add("Frequency", 100, HorizontalAlignment.Right)  // Width for the "Frequency" column
listView.Sorting <- SortOrder.Ascending
listView.Scrollable <- true
tableLayoutPanel.Controls.Add(listView, 0, 1)

let buttonPanel = new Panel(Dock = DockStyle.Fill)
let loadButton = TextAnalyzer.LoadTxt.createLoadButton()  
buttonPanel.Controls.Add(loadButton)
let centerButton () =
    loadButton.Location <- Point(
        (buttonPanel.Width - loadButton.Width) / 2,  (buttonPanel.Height - loadButton.Height) / 2 )
centerButton()
buttonPanel.Resize.Add(fun _ -> centerButton())
tableLayoutPanel.Controls.Add(buttonPanel, 0, 2)

let resultLabel = new Label(Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 10.0f))
tableLayoutPanel.Controls.Add(resultLabel, 0, 3)

form.Controls.Add(tableLayoutPanel)

let openFileDialog = new OpenFileDialog(Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*")

loadButton.Click.Add(fun _ -> 
    if openFileDialog.ShowDialog() = DialogResult.OK then
        let filePath = openFileDialog.FileName
        try
            let fileContent = File.ReadAllText(filePath)
            textBox.Text <- fileContent
            
            // Count words and sentences after loading the file
            let wordCount = CountWord(fileContent)
            let sentenceCount = CountSentence(fileContent)
            let paragraphCount = CountParagraphs(fileContent)
            let wordFrequency = CalculateWordFrequency(fileContent) |> Seq.toList
            let mostFrequent = MostFrequentWords(fileContent) |> Seq.toList

            // Format word frequency and most frequent words for display
            let wordFrequencyDisplay =
                wordFrequency
                |> List.map (fun (word, count) -> sprintf "%s: %d" word count)
                |> String.concat ", "

            let mostFrequentDisplay =
                mostFrequent
                |> List.map (fun (word, count) -> sprintf "%s: %d" word count)
                |> String.concat ", "
            
             // Display results in the Label
            resultLabel.Text <- sprintf "Words: %d | Sentences: %d | Paragraphs: %d\nMost Frequent Words: %s" wordCount sentenceCount paragraphCount mostFrequentDisplay

            // Populate the ListView with word frequencies
            listView.Items.Clear()
            wordFrequency
            |> List.iter (fun (word, count) -> 
                let listItem = new ListViewItem(word)
                listItem.SubItems.Add(count.ToString()) |> ignore
                listView.Items.Add(listItem) |> ignore
            )

        with
        | ex -> MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)

[<STAThread>]
do Application.Run(form)