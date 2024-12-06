open System
open System.Drawing
open System.Windows.Forms

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

let bottomPanel = new Panel(Dock = DockStyle.Bottom, Height = 60)
form.Controls.Add(bottomPanel)

[<STAThread>]
do Application.Run(form)
