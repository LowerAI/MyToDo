using MyToDo.Service;
using MyToDo.Shared.Dtos;

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
        public MemoViewModel(IMemoService service)
        {
            _service = service;
            MemoDtos = new();
            AddCommand = new DelegateCommand(Add);
            CreateToDoList();
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
        private readonly IMemoService _service;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        async void CreateToDoList()
        {
            var memoResult = await _service.GetAllAsync(new Shared.Parameters.QueryParameters
            {
                PageIndex = 0,
                PageSize = 100,
            });

            if (memoResult.Status)
            {
                foreach (var item in memoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
        }
    }
}