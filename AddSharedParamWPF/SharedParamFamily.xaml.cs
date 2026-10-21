using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AddSharedParamWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SharedParamFamily : Window
    {
        private Autodesk.Revit.DB.Document _doc;
        private List<SharedParamRow> _paramList;

        public SharedParamFamily()
        {
            InitializeComponent();
        }

        public SharedParamFamily(Autodesk.Revit.DB.Document doc)
        {
            InitializeComponent();
            _doc = doc;

            LoadSharedParameters();
            FillParameterGroups();
        }

        private void LoadSharedParameters()
        {
            _paramList = new List<SharedParamRow>();

            Autodesk.Revit.DB.DefinitionFile fopFile = _doc.Application.OpenSharedParameterFile();

            if (fopFile == null || string.IsNullOrEmpty(fopFile.Filename))
            {
                TxtFopPath.Text = "ФОП не подключен в настройках Revit!";
                BtnAdd.IsEnabled = false;
                return;
            }

            TxtFopPath.Text = fopFile.Filename;

            foreach (Autodesk.Revit.DB.DefinitionGroup group in fopFile.Groups)
            {
                foreach (Autodesk.Revit.DB.ExternalDefinition definition in group.Definitions)
                {
                    _paramList.Add(new SharedParamRow
                    {
                        IsSelected = false,
                        Name = definition.Name,
                        GroupName = group.Name,
                        Definition = definition
                    });
                }
            }

            _paramList = _paramList.OrderBy(p => p.GroupName).ThenBy(p => p.Name).ToList();
            LvParameters.ItemsSource = _paramList;
        }

        private void FillParameterGroups()
        {
            var groupList = new List<object>();

            foreach (Autodesk.Revit.DB.BuiltInParameterGroup group in Enum.GetValues(typeof(Autodesk.Revit.DB.BuiltInParameterGroup)))
            {
                try
                {
                    if (group == Autodesk.Revit.DB.BuiltInParameterGroup.INVALID || group.ToString().StartsWith("Create"))
                        continue;

                    string localName = Autodesk.Revit.DB.LabelUtils.GetLabelFor(group);

                    if (!string.IsNullOrEmpty(localName) && !groupList.Any(g => ((dynamic)g).Name == localName))
                    {
                        groupList.Add(new { Name = localName, Group = group });
                    }
                }
                catch { }
            }

            var sortedGroups = groupList.OrderBy(g => ((dynamic)g).Name).ToList();

            CbParameterGroups.DisplayMemberPath = "Name";
            CbParameterGroups.SelectedValuePath = "Group";
            CbParameterGroups.ItemsSource = sortedGroups;

            var defaultGroup = sortedGroups.FirstOrDefault(g => ((dynamic)g).Name == "Текст");
            if (defaultGroup != null) CbParameterGroups.SelectedItem = defaultGroup;
            else if (sortedGroups.Count > 0) CbParameterGroups.SelectedIndex = 0;
        }
        // ЗАМЕНИТЬ ТЕКУЩИЙ МЕТОД КНОПКИ НА ЭТОТ:
        private void BtnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedRows = _paramList.Where(p => p.IsSelected).ToList();

            if (selectedRows.Count == 0)
            {
                System.Windows.MessageBox.Show("Вы не выбрали ни одного параметра!", "Внимание", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // Считываем выбранную группу из ComboBox
            Autodesk.Revit.DB.BuiltInParameterGroup selectedGroup = (Autodesk.Revit.DB.BuiltInParameterGroup)CbParameterGroups.SelectedValue;

            int addedCount = 0;
            int skippedCount = 0;

            Autodesk.Revit.DB.FamilyManager familyManager = _doc.FamilyManager;

            using (Autodesk.Revit.DB.Transaction trans = new Autodesk.Revit.DB.Transaction(_doc, "Пакетное добавление параметров из ФОП"))
            {
                trans.Start();

                foreach (var row in selectedRows)
                {
                    try
                    {
                        Autodesk.Revit.DB.FamilyParameter existingParam = familyManager.get_Parameter(row.Name);
                        if (existingParam != null)
                        {
                            skippedCount++;
                            continue;
                        }

                        // Передаем выбранную группу в Revit
                        familyManager.AddParameter(row.Definition, selectedGroup, false);

                        addedCount++;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Ошибка добавления параметра {row.Name}: {ex.Message}");
                    }
                }

                trans.Commit();
            }

            System.Windows.MessageBox.Show($"Операция успешно завершена!\n\nДобавлено параметров: {addedCount}\nПропущено: {skippedCount}", "Успех", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            Close();
        }



    }
}
