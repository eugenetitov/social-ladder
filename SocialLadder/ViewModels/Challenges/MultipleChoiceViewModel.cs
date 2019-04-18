using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.Models.LocalModels.Mappers;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class MultipleChoiceViewModel : BaseChallengesViewModel
    {
        
        #region Propertyes
        private MvxObservableCollection<LocalChallengeAnswerModel> _multipleChoiceSource;
        public MvxObservableCollection<LocalChallengeAnswerModel> MultipleChoiceSource
        {
            get => _multipleChoiceSource;
            set => SetProperty(ref _multipleChoiceSource, value);
        }
        #endregion

        public MultipleChoiceViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger = null) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            TopMarginToCompleteView = true;
            HexCompleteViewColor = ChallengesConstants.ChallengesDetailsMCBackground;
            IsBusy = true;
        }

        public override void SetChallengeModel(ChallengeResponseModel response)
        {
            base.SetChallengeModel(response);
            MultipleChoiceSource = new MvxObservableCollection<LocalChallengeAnswerModel>();
            var mc_list = Challenge != null ? Challenge.AnswerList : null;
            if (mc_list == null || mc_list.Count == 0)
            {
                //HardCode
                //MultipleChoiceSource.Add(ChallengeAnswerModelMapper.ItemToLocalItem(new LocalChallengeAnswerModel { AnswerCode = "0", AnswerName = "Answer 0", ChallengeID = 0, isWriteIn = true, Sequence = 0 }));
                //MultipleChoiceSource.Add(ChallengeAnswerModelMapper.ItemToLocalItem(new LocalChallengeAnswerModel { AnswerCode = "1", AnswerName = "Answer 1", ChallengeID = 1, isWriteIn = true, Sequence = 1 }));
                //MultipleChoiceSource.Add(ChallengeAnswerModelMapper.ItemToLocalItem(new LocalChallengeAnswerModel { AnswerCode = "2", AnswerName = "Answer 2", ChallengeID = 2, isWriteIn = true, Sequence = 2 }));
                //MultipleChoiceSource.Add(ChallengeAnswerModelMapper.ItemToLocalItem(new LocalChallengeAnswerModel { AnswerCode = "3", AnswerName = "Answer 3", ChallengeID = 3, isWriteIn = true, Sequence = 3 }));
                return;
            }
            if (mc_list != null && mc_list.Count > 0)
            {
                foreach (var item in mc_list)
                {
                    MultipleChoiceSource.Add(ChallengeAnswerModelMapper.ItemToLocalItem(item));
                }
            }
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            SubmitButtonAnimated = true;
            await base.ChallengeSubmit();
            if (Challenge == null && Challenge.AnswerList == null)
            {
                SubmitButtonAnimated = false;
                return;
            }
            if (MultipleChoiceSource.Where(x => x.IsSelected).Count() > 0)
            {
                ChallengeAnswerModel answer = MultipleChoiceSource.Where(x => x.IsSelected).FirstOrDefault();
                if (answer == null)
                {
                    SubmitButtonAnimated = false;
                    return;
                }
                string userAnswer = null;
                if (answer.isWriteIn)
                {
                    userAnswer = await _alertService.ShowAlertWithInput(string.Empty, answer.writeInPrompt, "Ok", "Cancel");
                }
                await SL.Manager.SubmitAnswerAsync(Challenge.ID, answer.ID, userAnswer, (ChallengeResponseModel response) =>
                {
                    SubmitChallengeComplete(true, response);
                });
            }
            SubmitButtonAnimated = false;
        }



        #region Commands       
        public MvxAsyncCommand<LocalChallengeAnswerModel> MultipleChoiceItemClick
        {
            get
            {
                return new MvxAsyncCommand<LocalChallengeAnswerModel>(async(param) =>
                {
                    IsBusy = true;
                    MultipleChoiceSource.Where(x => x.IsSelected).All(x => x.IsSelected = false);
                    var selectedItem = param;
                    selectedItem.IsSelected = true;
                    var index = MultipleChoiceSource.IndexOf(param);
                    MultipleChoiceSource[index] = selectedItem;
                    IsBusy = false;
                });
            }
        }
        #endregion
    }
}
