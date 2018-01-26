using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
/*
Используя подходящие классы-коллекции из пространств имен System.Collections и System.Collections.Generic 
выполнить соответствующий вариант задания (Таблица №1), дополнив его возможностями сохранения и чтения данных в/из файлов. 
Предусмотреть возможность сериализации данных, как в двоичном формате, так и в формате SOAP/XML.

Написать двоичное дерево поиска произвольных объектов, обладающее возможность добавления, 
удаления и поиска элементов. Для сравнения объектов между собой использовать делегат, 
передаваемый при создании дерева.
*/
namespace Lab08_02
{
	[Serializable]
	public class Node
	{
		[XmlAttribute("Ключ-данные")]
		public int iData; // ключ
		[XmlAttribute("Другие-данные")]
		public double dData; //данные
		public Node leftChild;
	//	[NonSerialized]
		public Node rightChild;

		public void displayNode() {
			Console.WriteLine("{" + iData + ". " + dData + "} ");
		}
	}
	internal class TreeApp
	{
		public delegate bool ComparisonDelegate(object x, object y);

		public static void Main(String[] args) {
			//	int value;
			ComparisonDelegate delegateMy = objComparison;
			Tree theTree = new Tree(delegateMy);
			//создание дерева, вставка узлов
			theTree.insert(50, 1.5);
			theTree.insert(25, 1.2);
			theTree.insert(75, 1.7);
			theTree.insert(12, 1.5);
			theTree.insert(37, 1.2);
			theTree.insert(43, 1.7);
			theTree.insert(30, 1.5);
			theTree.insert(33, 1.2);
			theTree.insert(87, 1.7);
			theTree.insert(93, 1.5);
			theTree.insert(97, 1.5);


			theTree.displayTree();
			theTree.SaveAsBinary();
			theTree.SaveAsXML();

			Tree obj = new Tree(); obj.OpenFromBinary();
			obj.displayTree();
			obj.OpenFromXML();
			obj.displayTree();
			Console.ReadKey();
		}
		public static bool objComparison(object x, object y) {
			return (int) x < (int) y;
		}


		

	private sealed class Tree
	{
		private Node root; // корень дерева
		public readonly ComparisonDelegate del;
		public Tree() { root = null; }
		public Tree(ComparisonDelegate del) {
				root = null; this.del = del;
			}
		


	    //поиск узла с заданным ключом (дерево не пустое)
		public Node find(int key) {
			Node current = root;
			while (current.iData != key) {
				if ( del(key,current.iData)) current = current.leftChild;
				else current = current.rightChild;
				if ( current == null ) return null;
			}
			return current;
		}


		//вставка узлов
		public void insert(int id, double dd) {
			Node newNode = new Node();
			newNode.iData = id;
			newNode.dData = dd;
			if ( root == null ) root = newNode;
			else {
				Node current = root;
				Node parent;
				while (true) { // внутренний выход из цикла
					parent = current;
					if ( del(id, current.iData)) // Двигаться налево?
					{
						current = current.leftChild;
						if ( current == null ) // Если достигнут конец цепочки,
						{ // вставить слева
							parent.leftChild = newNode;
							return;
						}
					} else // Или направо?
					  {
						current = current.rightChild;
						if ( current == null ) // Если достигнут конец цепочки,
						{ // вставить справа
							parent.rightChild = newNode;
							return;
						}
					}
				}
			}
		}


		//удаление узла
		public bool delete(int key) {
			Node current = root;
			Node parent = root;
			bool isLeftChild = true;
			while ( current.iData != key ) // Поиск узла
			{
				parent = current;
				if ( del(key, current.iData) ) // Двигаться налево?
				{
					isLeftChild = true;
					current = current.leftChild;
				} else // Или направо?
				  {
					isLeftChild = false;
					current = current.rightChild;
				}
				if ( current == null ) return false;// Конец цепочки
													// Узел не найден
			}
			// Удаляемый узел найден


			// Если узел не имеет потомков, он просто удаляется.
			if ( current.leftChild == null && current.rightChild == null ) {
				if ( current == root ) // Если узел является корневым,
					root = null; // дерево очищается
				else if ( isLeftChild )
					parent.leftChild = null; // Узел отсоединяется
				else // от родителя
					parent.rightChild = null;
			}
					// Если нет правого потомка, узел заменяется левым поддеревом
			else if ( current.rightChild == null )
				if ( current == root )
					root = current.leftChild;
				else if ( isLeftChild )
					parent.leftChild = current.leftChild;
				else
					parent.rightChild = current.leftChild;
			// Если нет левого потомка, узел заменяется правым поддеревом
			else if ( current.leftChild == null )
				if ( current == root )
					root = current.rightChild;
				else if ( isLeftChild )
					parent.leftChild = current.rightChild;
				else
					parent.rightChild = current.rightChild;
			else // Два потомка, узел заменяется преемником
				{
				// Поиск преемника для удаляемого узла (current)
				Node successor = getSuccessor(current);
				// Родитель current связывается с посредником
				if ( current == root )
					root = successor;
				else if ( isLeftChild )
					parent.leftChild = successor;
				else
					parent.rightChild = successor; }
				// Преемник связывается с левым потомком current
				return true; // Признак успешного завершения
			}

		// -------------------------------------------------------------
		// Метод возвращает узел со следующим значением после delNode.
		// Для этого он сначала переходит к правому потомку, а затем
		// отслеживает цепочку левых потомков этого узла.
		private Node getSuccessor(Node delNode) {
			Node successorParent = delNode;
			Node successor = delNode;
			Node current = delNode.rightChild; // Переход к правому потомку
			while ( current != null ) // Пока остаются левые потомки
			{
				successorParent = successor;
				successor = current;
				current = current.leftChild; // Переход к левому потомку
			}
			// Если преемник не является
			if ( successor != delNode.rightChild ) // правым потомком,
			{ // создать связи между узлами
				successorParent.leftChild = successor.rightChild;
				successor.rightChild = delNode.rightChild;
			}
			return successor;
		}
		// -------------------------------------------------------------
		
		// -------------------------------------------------------------
		private void preOrder(Node localRoot) {
			if ( localRoot != null ) {
				Console.WriteLine(localRoot.iData + " ");
				preOrder(localRoot.leftChild);
				preOrder(localRoot.rightChild);
			}
		}
		// -------------------------------------------------------------
		// симметричный обход 
		private void inOrder(Node localRoot) {
			if ( localRoot != null ) {
				inOrder(localRoot.leftChild);
				Console.WriteLine(localRoot.iData + " ");
				inOrder(localRoot.rightChild);
			}
		}
		// -------------------------------------------------------------
		private void postOrder(Node localRoot) {
			if ( localRoot != null ) {
				postOrder(localRoot.leftChild);
				postOrder(localRoot.rightChild);
				Console.WriteLine(localRoot.iData + " ");
			}
		}
		// -------------------------------------------------------------

		public void displayTree() {
			Stack globalStack = new Stack();
			globalStack.Push(root);
			int nBlanks = 32;
			bool isRowEmpty = false;
			Console.WriteLine("......................................................");
			while ( isRowEmpty == false ) {
				Stack localStack = new Stack();
				isRowEmpty = true;
			for ( int j = 0; j < nBlanks; j++ )
				Console.Write(' ');
			while (globalStack.Count !=0 ) {
				Node temp = (Node) globalStack.Pop();
				if ( temp != null ) {
					Console.Write(temp.iData);
					localStack.Push(temp.leftChild);
					localStack.Push(temp.rightChild);
					if ( temp.leftChild != null ||
					temp.rightChild != null )
						isRowEmpty = false;
				} else {
					Console.Write("--");
					localStack.Push(null);
					localStack.Push(null);
				}
				for ( int j = 0; j < nBlanks * 2 - 2; j++ )
					Console.Write(' ');
			}
			Console.WriteLine();
			nBlanks /= 2;
			while ( localStack.Count != 0 )
				globalStack.Push(localStack.Pop());
		}
		Console.WriteLine("......................................................");
	}
		// -------------------------------------------------------------

		public void SaveAsBinary() {
			Stream st =
				new FileStream(
					"C:/Users/Admin/Desktop/7семестрc#/ВУЗ/Lab08_01/Lab08_02/treeBinary.txt",
					FileMode.Create);
			IFormatter f = new BinaryFormatter();
			f.Serialize(st, root);
			Console.WriteLine("Дерево сериализовано.");
			st.Close();
		}

		public void OpenFromBinary() {
			Stream st =
				new FileStream(
					"C:/Users/Admin/Desktop/7семестрc#/ВУЗ/Lab08_01/Lab08_02/treeBinary.txt",
					FileMode.Open);
			IFormatter f = new BinaryFormatter();
			root = (Node) f.Deserialize(st);
			st.Close();
		}
		public void SaveAsXML() {
			Stream st =
				new FileStream(
					"C:/Users/Admin/Desktop/7семестрc#/ВУЗ/Lab08_01/Lab08_02/treeXML.txt",
					FileMode.Create);
			XmlSerializer f = new XmlSerializer(root.GetType());
			f.Serialize(st, root);
			st.Close();
		}

		public void OpenFromXML() {
			Stream st =
				new FileStream(
						"C:/Users/Admin/Desktop/7семестрc#/ВУЗ/Lab08_01/Lab08_02/treeXML.txt",
					FileMode.Open);
			XmlSerializer f = new XmlSerializer(root.GetType());
			XmlReader reader = XmlReader.Create(st);
			root = (Node) f.Deserialize(reader);
			st.Close();
		}

	} // Конец класса Tree
	}
}
		