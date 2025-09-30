using magicSorter;
using magicSorter.Models;
using MtgApiManager.Lib.Model;
using static System.Net.Mime.MediaTypeNames;

namespace MtgSorterUI
{
   public partial class MtgSorterForm : Form
   {
      private readonly SortingUnit sortingUnit = new();
      public MtgSorterForm()
      {
         InitializeComponent();
         if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
         {
            this.treeView1.NodeMouseDoubleClick += TreeNodeMouseDoubleClick;
         }
      }

      private async void SubmitButtonClick(object sender, EventArgs e)
      {
         await LockButtons(true);
         this.submitButton.Enabled = false;
         List<string> input = this.richTextBox1.Lines.ToList();
         await sortingUnit.ProcessCards(input);
         var formattedCards = await sortingUnit.GetFormattedCards();
         var unrecognisedStrings = await sortingUnit.GetUnrecognised();

         this.richTextBox2.Lines = formattedCards.ToArray();
         this.richTextBox1.Lines = unrecognisedStrings.ToArray();
         await this.FillTree();
         await LockButtons(false);
         addButton.Enabled = true;
      }

      private async Task FillTextBox() //TODO: make genaric and use in submit and add.
      {
         var formattedCards = await sortingUnit.GetFormattedCards();
         this.richTextBox2.Lines = formattedCards.ToArray();
      }

      private async Task FillTree()
      {
         treeView1.BeginUpdate();
         try
         {
            var anchorNodeId = GetTreeNodeID(treeView1.SelectedNode);
            treeView1.Nodes.Clear();
            await sortingUnit.GenerateCardTree();
            CardTreeModel temp = await sortingUnit.GetCardTree();
            var bold = new Font(treeView1.Font, FontStyle.Bold);

            var root = new TreeNode("Cards") { 
               NodeFont = new Font(treeView1.Font, FontStyle.Bold),
               Name = "Cards"
            };
            /*
            var rareRoot = new TreeNode("Rares") { NodeFont = new Font(treeView1.Font, FontStyle.Bold) };
            foreach (var rareDict in temp.SortedRares)
            {
               var parentNode = new TreeNode(rareDict.Key) { NodeFont = new Font(treeView1.Font, FontStyle.Bold) };
               foreach(var rare in rareDict.Value)
               {
                  var node = new TreeNode(rare.Name) { Tag = rare };
                  parentNode.Nodes.Add(node);
               }
               rareRoot.Nodes.Add(parentNode);
            }
            treeView1.Nodes.Add(rareRoot);
            */

            //New rares
            var rareRoot = new TreeNode("Rares") { 
               NodeFont = new Font(treeView1.Font, FontStyle.Bold),
               Name = "Rares"
            };

            foreach(RareCard treeRares in temp.RaresNewSorted)
            {
               var parentNode = new TreeNode(treeRares.ColorIdentity) { NodeFont = new Font(treeView1.Font, FontStyle.Bold) };
               foreach(var rare in treeRares.RareCards)
               {
                  var node = new TreeNode($"{rare.Amount} {rare.RareCard.Name}") { 
                     Tag = rare,
                     Name = (rare.RareCard.Name + treeRares.ColorIdentity)
                  };
                  parentNode.Nodes.Add(node);
               }
               rareRoot.Nodes.Add(parentNode);
            }

            treeView1.Nodes.Add(rareRoot);

            var commonRoot = new TreeNode("Commons") { 
               NodeFont = new Font(treeView1.Font, FontStyle.Bold),
               Name = "Commons"
            };
            var sortedSets = temp.CommonSets.OrderByDescending(s => s.SetDate);
            foreach(CardTreeSet set in sortedSets)
            {
               var comParentNode = new TreeNode(set.SetName) { 
                  NodeFont = new Font(treeView1.Font, FontStyle.Bold),
                  Name = set.SetName
               };
               foreach (var card in set.Cards)
               {
                  var commonNode = new TreeNode($"{card.Amount} {card.Card.Name}") { 
                     Tag = card,
                     Name = (card.Card.Name + set.SetName )
                  };
                  comParentNode.Nodes.Add(commonNode);
               }
               commonRoot.Nodes.Add(comParentNode);
            }
            //root.Nodes.Add(commonRoot);
            treeView1.Nodes.Add(commonRoot);
            treeView1.ExpandAll();
            
            if (anchorNodeId != null)
            {
               TreeNode? restoreNode = treeView1.Nodes.Find(anchorNodeId, true).FirstOrDefault();
               if (restoreNode != null)
               {
                  treeView1.SelectedNode = restoreNode;
                  treeView1.TopNode = restoreNode;
               } else if(treeView1.Nodes.Count > 0)
               {
                  treeView1.TopNode = treeView1.Nodes[0];  
               }  
            }
            else if (treeView1.Nodes.Count > 0)
            {
               if (treeView1.Nodes.Count > 0)
               {
                  treeView1.TopNode = treeView1.Nodes[0];
               }
            }
         }
         finally
         {
            treeView1.EndUpdate();
         }
      }

      private string GetTreeNodeID(TreeNode node)
      {
         if(node == null)
         {
            return null;
         }

         int spaceIndex = node.Text.IndexOf(' ');
         int cardAmount;
         if(int.TryParse(node.Text.Substring(0, spaceIndex), out int amount)){
            cardAmount = amount;
         }
         else
         {
            cardAmount = 1;
         }

         if(cardAmount > 1)
         {
            return node.Name;
         }
         var closestNode = node.NextNode;

         if(node.PrevNode != null)
         {
            return node.PrevNode.Name;
         }

         TreeNode parent = node.Parent;

         while (true)
         {
            parent = parent.NextNode;
            if(parent == null)
            {
               break;
            }
            foreach(TreeNode nodeTest in parent.Nodes)
            {
               if(nodeTest.Text != node.Text)
               {
                  return nodeTest.Name;
               }
            }
         }

         parent = node.Parent;
         while(true)
         {
            parent = parent.PrevNode;
            if (parent == null)
            {
               break;
            }
            foreach (TreeNode nodeTest in parent.Nodes)
            {
               if (nodeTest.Text != node.Text)
               {
                  return nodeTest.Name;
               }
            }
         }

         return null;
      }
      

      private async void RemoveCardFromTree(TreeNode node)
      {
         if (node == null || node.Parent == null) return; // Don't remove headers or root nodes
         //TODO make tag the name so this isn't needed
         if (node.Tag is CardTreeRare rare)
         {
            await sortingUnit.RemoveCard(rare.RareCard.Name);
         }
         else if (node.Tag is CardTreeCard commonCard)
         {
            await sortingUnit.RemoveCard(commonCard.Card.Name);
         }
         else
         {
            return;
         }
         await FillTree();
         await FillTextBox();
      }

      private void TreeNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         RemoveCardFromTree(e.Node);
      }

      private async void ClearStoredCards(object sender, EventArgs e)
      {
         await sortingUnit.Clear();
         this.richTextBox1.Clear();
         this.richTextBox2.Clear();
         this.treeView1.Nodes.Clear();
         this.LockButtons(true);
         this.submitButton.Enabled = true;
      }

      private async void ConvertButtonCLick(object sender, EventArgs e)
      {
         var cardStrings = await sortingUnit.GetArchidectCards();
         this.richTextBox2.Clear();
         this.richTextBox2.Lines = cardStrings.ToArray();
      }

      private async void SortedListButtonClick(object sender, EventArgs e)
      {
         var formattedCards = await sortingUnit.GetFormattedCards();
         this.richTextBox2.Clear();
         this.richTextBox2.Lines = formattedCards.ToArray();
      }

      private async void AddButtonCLick(object sender, EventArgs e)
      {
         this.addButton.Enabled = false;
         await LockButtons(true);
         List<string> input = this.richTextBox1.Lines.ToList();
         await sortingUnit.AddCards(input);

         var formattedCards = await sortingUnit.GetFormattedCards();
         var unrecognisedStrings = await sortingUnit.GetUnrecognised();
         await FillTree();

         this.richTextBox2.Lines = formattedCards.ToArray();
         this.richTextBox1.Lines = unrecognisedStrings.ToArray();
         this.addButton.Enabled = true;
         await LockButtons(false);
      }

      private async Task LockButtons(bool lockButtons)
      {
         if (lockButtons) {
            this.listButton.Enabled = false;
            this.archidectButton.Enabled = false;
            this.clearButton.Enabled = false;
            //this.button4.Enabled = false;
         } else
         {
            this.listButton.Enabled = true;
            this.archidectButton.Enabled = true;
            this.clearButton.Enabled = true;
            //this.button4.Enabled = true;
         }

      }
    }
}