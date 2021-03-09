using AgendaTelefonica.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaTelefonica
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Display();
        }
        /// 
        /// MOSTRA OS DADOS DO BANCO NO DATAGRIDVIEW
        /// 
        public void Display()
        {
            using (agendaTelefonicaEntities _entity = new agendaTelefonicaEntities())
            {
                List<Contatos> listaDeContatos = new List<Contatos>();
                listaDeContatos = _entity.tb_contatos.Select(x => new Contatos { Id = x.id, Nome = x.nome, Telefone = x.telefone }).ToList();
                dgvContatos.DataSource = listaDeContatos;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            tb_contatos contato = new tb_contatos();
            contato.nome = txtNome.Text;
            contato.telefone = txtTelefone.Text;
            bool result = SalvarContato(contato);
            ShowStatus(result, "Novo");
        }
        public bool SalvarContato(tb_contatos contato)
        {
            bool result = false;
            using (agendaTelefonicaEntities _entity = new agendaTelefonicaEntities())
            {
                _entity.tb_contatos.Add(contato);
                _entity.SaveChanges();
                result = true;
            }
            return result;
        }
        ///
        /// Validar o status da operação e mostrar as mensagens ao usuário
        ///
        public void ShowStatus(bool result, string Action)
        {
            if (result)
            {
                if (Action.ToUpper() == "NOVO")
                {
                    MessageBox.Show("Contato Salvo!", "Novo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Action.ToUpper() == "EDITAR")
                {
                    MessageBox.Show("Contato Editado!", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Contato Excluido!", "Excluir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Houve algum erro! Por favor tente novamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LimparCampos();
            Display();
        }
        /// 
        /// Limpar os campos após a operação de Inserir ou Atualizar ou Excluir
        ///
        public void LimparCampos()
        {
            txtNome.Text = "";
            txtTelefone.Text = "";
        }
        /// <summary>
        /// Valores da linha selecionada
        /// </summary>
        private void dgvContatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvContatos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvContatos.SelectedRows)
                {
                    lbl_Id.Text = row.Cells[0].Value.ToString();
                    txtNome.Text = row.Cells[1].Value.ToString();
                    txtTelefone.Text = row.Cells[2].Value.ToString();
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            tb_contatos contato = SetValues(Convert.ToInt32(lbl_Id.Text), txtNome.Text, txtTelefone.Text);
            bool result = EditarContato(contato);
            ShowStatus(result, "EDITAR");
        }
        public bool EditarContato(tb_contatos contato)
        {
            bool result = false;
            using (agendaTelefonicaEntities _entity = new agendaTelefonicaEntities())
            {
                tb_contatos _contato = _entity.tb_contatos.Where(x => x.id == contato.id).Select(x => x).FirstOrDefault();
                _contato.id = contato.id;
                _contato.nome = contato.nome;
                _contato.telefone = contato.telefone;
                _entity.SaveChanges();
                result = true;
            }
            return result;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            tb_contatos contato = SetValues(Convert.ToInt32(lbl_Id.Text), txtNome.Text, txtTelefone.Text);
            bool result = ExcluirContato(contato);
            ShowStatus(result, "EXCLUIR");
        }
        public bool ExcluirContato(tb_contatos contato)  
        {
            bool result = false;
            using (agendaTelefonicaEntities _entity = new agendaTelefonicaEntities())
            {
                tb_contatos _contato = _entity.tb_contatos.Where(x => x.id == contato.id).Select(x => x).FirstOrDefault();
                _entity.tb_contatos.Remove(_contato);
                _entity.SaveChanges();
                result = true;
            }
            return result;
        }
        public tb_contatos SetValues(int id, string nome, string telefone)
        {
            tb_contatos contato = new tb_contatos
            {
                id = id,
                nome = nome,
                telefone = telefone
            };
            return contato;
        }
    }
}
