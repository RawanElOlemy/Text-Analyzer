﻿open System
open System.IO
open System.Drawing
open System.Windows.Forms
open TextAnalyzer.LoadTxt

let form = new Form(Text = "Text Analyzer", Width = 600, Height = 400, StartPosition = FormStartPosition.CenterScreen)

let textBoxPanel = new Panel(Dock = DockStyle.Top, Height = 250, Padding = Padding(10))
let textBox = new TextBox(
    Multiline = true, 
    Dock = DockStyle.Fill, 
    ScrollBars = ScrollBars.Vertical, 
    BorderStyle = BorderStyle.FixedSingle, 
    Font = new Font("Consolas", 10.0f)
)
textBoxPanel.Controls.Add(textBox)
form.Controls.Add(textBoxPanel)

let buttonPanel = new Panel(Dock = DockStyle.Bottom, Height = 60)
form.Controls.Add(buttonPanel)


let loadButton = TextAnalyzer.LoadTxt.createLoadButton()  
buttonPanel.Controls.Add(loadButton)

let centerButton () =
    loadButton.Location <- Point(
        (buttonPanel.Width - loadButton.Width) / 2,  (buttonPanel.Height - loadButton.Height) / 2 )

centerButton()

buttonPanel.Resize.Add(fun _ -> centerButton())

let openFileDialog = new OpenFileDialog(Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*")

loadButton.Click.Add(fun _ ->
    if openFileDialog.ShowDialog() = DialogResult.OK then
        let filePath = openFileDialog.FileName
        try
            let fileContent = File.ReadAllText(filePath)
            textBox.Text <- fileContent
        with
        | ex -> MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)

[<STAThread>]
do Application.Run(form)