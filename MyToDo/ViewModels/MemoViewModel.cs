using MyToDo.Common.Models;

using Prism.Commands;
using Prism.Mvvm;

using System.Collections.ObjectModel;

namespace MyToDo.ViewModels
{
    /// <summary>
    /// 备忘录实体类
    /// </summary>
    public class MemoViewModel : BindableBase
    {
        public MemoViewModel()
        {
            MemoDtos = new();
            CreateToDoList();
            AddCommand = new DelegateCommand(Add);
        }

        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        private bool isRightDrawerOpen;
        /// <summary>
        /// 右侧添加窗口是否展开
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        public DelegateCommand AddCommand { get; private set; }

        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        void CreateToDoList()
        {
            for (int i = 0; i < 8; i++)
            {
                MemoDtos.Add(new MemoDto { Title = $"标题{i}", Content = "测试数据...." });
            }
        }
    }
}