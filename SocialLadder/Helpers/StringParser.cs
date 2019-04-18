using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Helpers
{
    public static class StringParser
    {
        private static int _utf16Emoji32HexStringLength = 12;
        private static int _utf16Emoji16HexStringLength = 6;
        private static int _utf16EmojiStringifiedHexCharLenght = 4;
        private static int _utf16EmojiFirstStringifiedCharPosition = 2;
        private static int _utf16EmojiSecondStringifiedCharPosition = 8;

        public static void ParseEmoji(FeedResponseModel feedResponse, string content, HttpResponseMessage response)
        {
            return;
            /*
            if (feedResponse?.FeedPage?.FeedItemList == null || feedResponse.FeedPage.FeedItemList.Count == 0)
            {
                return;
            }

            

            string emojiToParse = "Sunny D\\u2600\\ufe0fSunny D";
            string nameToParse = string.Empty;
            string parsedName = string.Empty;
            List<char> emojiCharacters = emojiToParse.ToCharArray().ToList();
            List<char> parsedEmojiCharacters = new List<char>();
            List<char> suggestedEmoji = new List<char>();
            char[] emojiChars = new char[] { '0', '0' };

            bool emojiParsed = false;
            int currentChar = 0;
            int startFrameToAdd = 0;
            int endFrameToAdd = 0;
            int emojiCharactersCount = emojiCharacters.Count;

            foreach (FeedItemModel feedItemModel in feedResponse.FeedPage.FeedItemList)
            {
                if (feedItemModel.Header?.Actor != null)
                {
                    nameToParse = feedItemModel.Header.Actor;
                    parsedName = string.Empty;

                    emojiCharacters = nameToParse.ToCharArray().ToList();
                    parsedEmojiCharacters.Clear();
                    suggestedEmoji.Clear();

                    currentChar = 0;
                    startFrameToAdd = 0;
                    endFrameToAdd = 0;
                    emojiCharactersCount = emojiCharacters.Count;

                    try
                    {
                        foreach (char item in emojiCharacters)
                        {
                            //check if emoji frame begins or if string ends
                            if (endFrameToAdd == emojiCharactersCount - 1)
                            {
                                parsedEmojiCharacters.AddRange(emojiCharacters.Skip(startFrameToAdd).Take(emojiCharactersCount - startFrameToAdd));
                                break;
                            }

                            if (currentChar < startFrameToAdd)
                            {
                                ++currentChar;
                                continue;
                            }

                            if (item != '\\')
                            {
                                ++currentChar;
                                ++endFrameToAdd;
                                continue;
                            }

                            //check for emoji
                            suggestedEmoji = emojiCharacters.Skip(endFrameToAdd).Take(_utf16Emoji16HexStringLength).ToList();
                            if (suggestedEmoji.Count < _utf16Emoji16HexStringLength)
                            {
                                parsedEmojiCharacters.AddRange(emojiCharacters.Skip(startFrameToAdd).Take(emojiCharactersCount - startFrameToAdd));
                                break;
                            }

                            emojiParsed = Is16BitEmoji(suggestedEmoji, out emojiChars);
                            if (emojiParsed)
                            {
                                //insert parsed emoji
                                parsedEmojiCharacters.AddRange(emojiCharacters.Skip(startFrameToAdd).Take(endFrameToAdd - startFrameToAdd));
                                //parsedEmojiCharacters.AddRange(emojiChars);

                                endFrameToAdd = startFrameToAdd = endFrameToAdd + _utf16Emoji16HexStringLength;
                                ++currentChar;

                                continue;
                            }

                            suggestedEmoji = emojiCharacters.Skip(endFrameToAdd).Take(_utf16Emoji32HexStringLength).ToList();
                            if (suggestedEmoji.Count < _utf16Emoji32HexStringLength)
                            {
                                parsedEmojiCharacters.AddRange(emojiCharacters.Skip(startFrameToAdd).Take(emojiCharactersCount - startFrameToAdd));
                                break;
                            }

                            emojiParsed = Is32BitEmoji(suggestedEmoji, out emojiChars);
                            if (emojiParsed)
                            {
                                //insert parsed emoji
                                parsedEmojiCharacters.AddRange(emojiCharacters.Skip(startFrameToAdd).Take(endFrameToAdd - startFrameToAdd));
                                parsedEmojiCharacters.AddRange(emojiChars);

                                endFrameToAdd = startFrameToAdd = endFrameToAdd + _utf16Emoji32HexStringLength;
                                ++currentChar;

                                continue;
                            }

                            ++currentChar;
                            ++endFrameToAdd;

                        }
                    }
                    catch (Exception ex)
                    {
                        //not change initial string
                        continue;
                    }

                    parsedName = new string(parsedEmojiCharacters.ToArray());

                    feedItemModel.Header.Actor = parsedName == string.Empty ? nameToParse : parsedName;
                }
            }*/
        }

        private static bool Is16BitEmoji(List<char> suggestedEmoji, out char[] emojiChars)
        {
            emojiChars = new char[] { '0' };

            //check emoji format
            if (suggestedEmoji.Count < _utf16Emoji16HexStringLength)
            {
                return false;
            }

            if (suggestedEmoji[_utf16EmojiFirstStringifiedCharPosition - 1] != 'u')
            {
                return false;
            }

            //get emoji numeric code
            try
            {
                int firstEmojiCharHexValue = int.Parse(new string(suggestedEmoji.Skip(_utf16EmojiFirstStringifiedCharPosition)
                     .Take(_utf16EmojiStringifiedHexCharLenght).ToArray()), System.Globalization.NumberStyles.HexNumber);

                emojiChars[0] = (char)firstEmojiCharHexValue;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static bool Is32BitEmoji(List<char> suggestedEmoji, out char[] emojiChars)
        {
            emojiChars = new char[] { '0', '0' };

            //check emoji format
            if (suggestedEmoji.Count < _utf16Emoji32HexStringLength)
            {
                return false;
            }

            if (suggestedEmoji[_utf16EmojiFirstStringifiedCharPosition - 1] != 'u'
    || suggestedEmoji[_utf16EmojiSecondStringifiedCharPosition - 2] != '\\'
    || suggestedEmoji[_utf16EmojiSecondStringifiedCharPosition - 1] != 'u')
            {
                return false;
            }

            //get emoji numeric code
            try
            {
                int firstEmojiCharHexValue = int.Parse(new string(suggestedEmoji.Skip(_utf16EmojiFirstStringifiedCharPosition)
                     .Take(_utf16EmojiStringifiedHexCharLenght).ToArray()), System.Globalization.NumberStyles.HexNumber);
                int secondEmojiCharHexValue = int.Parse(new string(suggestedEmoji.Skip(_utf16EmojiSecondStringifiedCharPosition)
                     .Take(_utf16EmojiStringifiedHexCharLenght).ToArray()), System.Globalization.NumberStyles.HexNumber);

                emojiChars[0] = (char)firstEmojiCharHexValue;
                emojiChars[1] = (char)secondEmojiCharHexValue;

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


    }
}
