using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Livro_C_Sharp___Capitulo_12__XML_
{
    public partial class Form1 : Form
    {
        TreeNode subNo; //nó para as turmas (segundo nível)
        string ficheiro = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\escola.xml";  //localização do ficheiro xml

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "XML";
            comboBox1.Items.Add("");
            comboBox1.Items.Add("14");
            comboBox1.Items.Add("15");
            comboBox1.Items.Add("16");
            comboBox1.Items.Add("17");
            comboBox1.Items.Add("18");

            //estilo da combo box
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            //legendas para os controlos
            label1.Text = "Nome";
            label2.Text = "Idade";
            groupBox1.Text = "Sexo";
            radioButton1.Text = "Masculino";
            radioButton2.Text = "Feminino";
            button1.Text = "Adicionar";
            button2.Text = "Remover";
            button3.Text = "Alterar";
            button4.Text = "Cancelar";
            LimparForm(); //reset ao conteudo
            CarregarTreeView();

        }

        void LimparForm()
        {
            textBox1.Text = null;
            comboBox1.Text = null;

            //reset aos radiobuttons
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            textBox1.Enabled = false;
            comboBox1.Enabled = false;
            groupBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;

        }

        void CarregarTreeView() {
            treeView1.Nodes.Add("Turmas");
            XmlTextReader xmlreader = new XmlTextReader(ficheiro);
            TreeNode noPrincipal = treeView1.Nodes[0];
            TreeNode subSubNo;
            while (xmlreader.Read()) {
                if (xmlreader.HasAttributes) {
                    while (xmlreader.MoveToNextAttribute()) {
                        if (xmlreader.Name == "ano") {
                            subNo = new TreeNode();
                            subNo.Text = xmlreader.Value;
                            noPrincipal.Nodes.Add(subNo);
                        }
                        if (xmlreader.Name == "letra") {
                            subNo.Text = subNo.Text + "º " + xmlreader.Value;
                        }
                        if (xmlreader.Name == "area") {
                            subNo.Text = subNo.Text + "( " + xmlreader.Value + ")";
                        }
                        if (xmlreader.Name == "nome") {
                            subSubNo = new TreeNode();
                            subSubNo.Text = xmlreader.Value;
                            subNo.Nodes.Add(subSubNo);
                        }
                    }
                }
            }
            xmlreader.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode = treeView1.Nodes["Turmas"];
            LimparForm();
            treeView1.Focus();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Level) {
                case 0:                     //selecção de nó rincilap
                        LimparForm();
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    break;
                case 1:                     //selecção de turma
                    LimparForm();
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    AtivarCampos();
                    break;
                case 2:                     //selecção de aluno
                    LimparForm();
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    AtivarCampos();
                    ApresentarDados();
                    break;
            }
            
        }

        void AtivarCampos() {
            textBox1.Enabled = true;
            comboBox1.Enabled = true;
            groupBox1.Enabled = true;
        }

        void ApresentarDados() {
            textBox1.Text = treeView1.SelectedNode.Text; //aluno seleccionado
            XmlTextReader xmlreader = new XmlTextReader(ficheiro);
            while (xmlreader.Read()) {
                if (xmlreader.HasAttributes) {
                    while (xmlreader.MoveToNextAttribute()) {
                        if (xmlreader.Value == treeView1.SelectedNode.Text) {
                            xmlreader.MoveToNextAttribute();
                            if (xmlreader.Value == "Masculino")
                            {
                                radioButton1.Checked = true;
                            }
                            else {
                                radioButton2.Checked = true;
                            }
                        }
                        xmlreader.MoveToNextAttribute();
                        comboBox1.Text = xmlreader.Value; //idade
                        break;
                    }
                }
            }
            xmlreader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidarDados() == true) {
                AdicionarDados();
            }
        }

        bool ValidarDados() {

            //validação do campo Nome
            if (textBox1.Text.Trim().Length == 0) {
                MessageBox.Show("Preencha o campo Nome");
                return false;
            }
            //validação do campo Idade
            if (comboBox1.Text == null) {
                MessageBox.Show("Preencha o campo Idade");
                return false;
            }
            //validação do campo Sexo
            if (radioButton1.Checked == false && radioButton2.Checked == false) {
                MessageBox.Show("Preencha o campo Sexo");
                return false;
            }
            return true;
        }

        void AdicionarDados() {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ficheiro);
            string turma, ano, letra;
            turma = treeView1.SelectedNode.Text;
            ano = turma.Substring(0, 1);
            letra = turma.Substring(3, 1);
            //String que contém o filtro a ser aplicado
            string filtro;
            filtro = "/escola/turma[@ano='"+ ano +"' and @letra='" + letra + "']";
            XmlElement elementoTurma;

            //Selecção de turma
            elementoTurma = (XmlElement)xmldoc.SelectSingleNode(filtro);
            XmlElement elementoAluno;
            //Criação do aluno (elemento e atributo)
            elementoAluno = xmldoc.CreateElement("aluno");
            XmlAttribute atributoNome;
            atributoNome = xmldoc.CreateAttribute("nome");
            atributoNome.Value = textBox1.Text;
            XmlAttribute atributoSexo;
            atributoSexo = xmldoc.CreateAttribute("sexo");
            if (radioButton1.Checked == true)
            {
                atributoSexo.Value = "Masculino";

            }
            else {
                atributoSexo.Value = "Feminino";
            }
            XmlAttribute atributoIdade;
            atributoIdade = xmldoc.CreateAttribute("idade");
            atributoIdade.Value = comboBox1.Text;
            elementoAluno.Attributes.Append(atributoNome);
            elementoAluno.Attributes.Append(atributoSexo);
            elementoAluno.Attributes.Append(atributoIdade);
            //Inclusão do aluno na turma
            xmldoc.Save(ficheiro);  //Gravação do ficheiro
            MessageBox.Show("O aluno foi adicionado com sucesso");
            treeView1.Nodes.Clear();   // Limpeza do controlo TreeView
            CarregarTreeView();   //Rearregamento do controlo TreeView
            LimparForm();   // Reset ao formulário
            treeView1.ExpandAll();  //expansão de todos os nós do controlo TreeView
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ValidarDados() == true) {
                AlterarDados();
            }
        }

        void AlterarDados() {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ficheiro);
            string turma, ano, letra;
            turma = treeView1.SelectedNode.Parent.Text;
            ano = turma.Substring(0, 1);
            letra = turma.Substring(3, 1);
            string filtro;
            //Selecção de turma
            XmlElement elementoTurma;
            filtro = "/escola/turma[@ano='" + ano + "' and @letra='" + letra + "']";
            elementoTurma = (XmlElement)xmldoc.SelectSingleNode(filtro);
            //Redefinição de valores
            XmlElement elementoAluno;
            string aluno;
            aluno = treeView1.SelectedNode.Text;
            filtro = filtro + "/aluno[@nome='" + aluno + "']";
            elementoAluno = (XmlElement)xmldoc.SelectSingleNode(filtro);
            //Redefinição de Valores
            XmlAttribute atributoNome;
            atributoNome = elementoAluno.Attributes["nome"];
            atributoNome.Value = textBox1.Text;
            XmlAttribute atributoSexo;
            atributoSexo = elementoAluno.Attributes["sexo"];
            if (radioButton1.Checked == true)
            {
                atributoSexo.Value = "Masculino";
            }
            else {
                atributoSexo.Value = "Feminino";
            }
            XmlAttribute atributoIdade;
            atributoIdade = elementoAluno.Attributes["idade"];
            atributoIdade.Value = comboBox1.Text;
            xmldoc.Save(ficheiro);   //grava o ficheiro XML
            MessageBox.Show("O aluno foi alterado com sucesso!!");
            treeView1.Nodes.Clear();  // limpa o controlo TreeView
            CarregarTreeView();       //Recarregamento do controlo TreeView
            LimparForm();   //Reset ao formulário inteiro
            treeView1.ExpandAll();  //Expansão de todos os nós do controlo TreeView



        }

        private void button2_Click(object sender, EventArgs e)
        {
            RemoverDados();
        }

        void RemoverDados() {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ficheiro);
            string turma, ano, letra;
            turma = treeView1.SelectedNode.Parent.Text;
            ano = turma.Substring(0, 1);
            letra  = turma.Substring(3, 1);
            string filtro;

            //Selecção de turma
            XmlElement elementoTurma;
            filtro = "/escola/turma[@ano='" + ano + "' and @letra='" + letra + "']";
            elementoTurma = (XmlElement)xmldoc.SelectSingleNode(filtro);

            //Selecção do Aluno
            XmlElement elementoAluno;
            string aluno;
            aluno = treeView1.SelectedNode.Text;
            filtro = filtro + "/aluno[@nome='" + aluno + "']";
            elementoAluno = (XmlElement)xmldoc.SelectSingleNode(filtro);
            elementoAluno.RemoveAll(); //Remoção do aluno
            xmldoc.Save(ficheiro);
            MessageBox.Show("O aluno foi removido com sucesso!");
            treeView1.Nodes.Clear();  //Limpeza do TreeView
            CarregarTreeView();  //Recarregar Treeview
            LimparForm();
            treeView1.ExpandAll();
        }
    }
}
