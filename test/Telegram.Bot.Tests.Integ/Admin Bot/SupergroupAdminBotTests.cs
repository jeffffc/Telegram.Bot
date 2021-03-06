using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Admin_Bot
{
    [Collection(Constants.TestCollections.SuperGroupAdminBots)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class SupergroupAdminBotTests : IClassFixture<AdminBotTestFixture>
    {
        private readonly AdminBotTestFixture _classFixture;

        private readonly TestsFixture _fixture;

        private ITelegramBotClient BotClient => _fixture.BotClient;

        public SupergroupAdminBotTests(TestsFixture testsFixture, AdminBotTestFixture classFixture)
        {
            _fixture = testsFixture;
            _classFixture = classFixture;
            _classFixture.ChatId = _fixture.SupergroupChat.Id;
        }

        #region 1. Changing Chat Title

        [OrderedFact(DisplayName = FactTitles.ShouldSetChatTitle)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatTitle)]
        public async Task Should_Set_Chat_Title()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldSetChatTitle);

            await BotClient.SetChatTitleAsync(
                chatId: _classFixture.ChatId,
                title: _classFixture.ChatTitle
            );
        }

        #endregion

        #region 2. Changing Chat Description

        [OrderedFact(DisplayName = FactTitles.ShouldSetChatDescription)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatDescription)]
        public async Task Should_Set_Chat_Description()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldSetChatDescription);

            await BotClient.SetChatDescriptionAsync(
                chatId: _classFixture.ChatId,
                description: "Test Chat Description"
            );
        }

        [OrderedFact(DisplayName = FactTitles.ShouldDeleteChatDescription)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatDescription)]
        public async Task Should_Delete_Chat_Description()
        {
            // ToDo: exception Bad Request: chat description is not modified

            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldDeleteChatDescription);

            await BotClient.SetChatDescriptionAsync(
                chatId: _classFixture.ChatId
            );
        }

        #endregion

        #region 3. Pinning Chat Description

        [OrderedFact(DisplayName = FactTitles.ShouldPinMessage)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.PinChatMessage)]
        public async Task Should_Pin_Message()
        {
            Message msg = await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldPinMessage);

            await BotClient.PinChatMessageAsync(
                chatId: _classFixture.ChatId,
                messageId: msg.MessageId
            );

            _classFixture.PinnedMessage = msg;
        }

        [OrderedFact(DisplayName = FactTitles.ShouldGetChatPinnedMessage)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChat)]
        public async Task Should_Get_Chat_Pinned_Message()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldGetChatPinnedMessage);

            Message pinnedMsg = _classFixture.PinnedMessage;

            Chat chat = await BotClient.GetChatAsync(_classFixture.ChatId);

            Assert.True(JToken.DeepEquals(
                JToken.FromObject(pinnedMsg), JToken.FromObject(chat.PinnedMessage)
            ));
        }

        [OrderedFact(DisplayName = FactTitles.ShouldUnpinMessage)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.UnpinChatMessage)]
        public async Task Should_Unpin_Message()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldUnpinMessage);

            await BotClient.UnpinChatMessageAsync(_classFixture.ChatId);
        }

        [OrderedFact(DisplayName = FactTitles.ShouldGetChatWithNoPinnedMessage)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChat)]
        public async Task Should_Get_Chat_With_No_Pinned_Message()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldGetChatWithNoPinnedMessage);

            Chat chat = await BotClient.GetChatAsync(_classFixture.ChatId);

            Assert.Null(chat.PinnedMessage);
        }

        #endregion

        #region 4. Changing Chat Photo

        [OrderedFact(DisplayName = FactTitles.ShouldSetChatPhoto)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatPhoto)]
        public async Task Should_Set_Chat_Photo()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldSetChatPhoto);

            using (Stream stream = System.IO.File.OpenRead(Constants.FileNames.Photos.Logo))
            {
                await BotClient.SetChatPhotoAsync(
                    chatId: _classFixture.ChatId,
                    photo: stream
                );
            }
        }

        [OrderedFact(DisplayName = FactTitles.ShouldDeleteChatPhoto)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteChatPhoto)]
        public async Task Should_Delete_Chat_Photo()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldDeleteChatPhoto);

            await BotClient.DeleteChatPhotoAsync(_classFixture.ChatId);
        }

        [OrderedFact(DisplayName = FactTitles.ShouldThrowOnDeletingChatDeletedPhoto)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteChatPhoto)]
        public async Task Should_Throw_On_Deleting_Chat_Deleted_Photo()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldThrowOnDeletingChatDeletedPhoto);

            Exception e = await Assert.ThrowsAnyAsync<Exception>(() =>
                BotClient.DeleteChatPhotoAsync(_classFixture.ChatId));

            // ToDo: Create exception type
            Assert.IsType<ApiRequestException>(e);
            Assert.Equal("Bad Request: CHAT_NOT_MODIFIED", e.Message);
        }

        #endregion

        #region 5. Chat Sticker Set

        [OrderedFact(DisplayName = FactTitles.ShouldThrowOnSettingChatStickerSet)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatStickerSet)]
        public async Task Should_Throw_On_Setting_Chat_Sticker_Set()
        {
            await _fixture.SendTestCaseNotificationAsync(FactTitles.ShouldThrowOnSettingChatStickerSet);

            const string setName = "EvilMinds";

            ApiRequestException exception = await Assert.ThrowsAnyAsync<ApiRequestException>(() =>
                _fixture.BotClient.SetChatStickerSetAsync(_classFixture.ChatId, setName)
            );

            // ToDo: Create exception type
            Assert.Equal(400, exception.ErrorCode);
            Assert.Equal("Bad Request: can't set supergroup sticker set", exception.Message);
        }

        #endregion

        private static class FactTitles
        {
            public const string ShouldSetChatTitle = "Should set chat title";

            public const string ShouldSetChatDescription = "Should set chat description";

            public const string ShouldDeleteChatDescription = "Should delete chat description";

            public const string ShouldPinMessage = "Should pin chat message";

            public const string ShouldGetChatPinnedMessage = "Should get chat's pinned message";

            public const string ShouldUnpinMessage = "Should unpin chat message";

            public const string ShouldGetChatWithNoPinnedMessage = "Should get the chat info without a pinned message";

            public const string ShouldSetChatPhoto = "Should set chat photo";

            public const string ShouldDeleteChatPhoto = "Should delete chat photo";

            public const string ShouldThrowOnDeletingChatDeletedPhoto =
                "Should throw exception in deleting chat photo with no photo currently set";

            public const string ShouldThrowOnSettingChatStickerSet =
                "Should throw exception when trying to set sticker set for a chat with less than 100 members";
        }
    }
}
