/*
 * CKFinder
 * ========
 * http://www.ckfinder.com
 * Copyright (C) 2007-2008 Frederico Caldeira Knabben (FredCK.com)
 *
 * The software, this file and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying or distribute this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 */

using System;
using System.IO;
using System.Web;

namespace CKFinder.Connector.CommandHandlers
{
    internal class ThumbnailCommandHandler : CommandHandlerBase
    {
        public override void SendResponse(HttpResponse response)
        {
            CheckConnector();

            try
            {
                CheckRequest();
            }
            catch (ConnectorException connectorException)
            {
                response.AddHeader("X-CKFinder-Error", (connectorException.Number).ToString());
                response.StatusCode = 403;
                response.End();
                return;
            }
            catch
            {
                response.AddHeader("X-CKFinder-Error", Errors.Unknown.ToString());
                response.StatusCode = 403;
                response.End();
                return;
            }

            if (!Config.Current.Thumbnails.Enabled)
            {
                response.AddHeader("X-CKFinder-Error", Errors.ThumbnailsDisabled.ToString());
                response.StatusCode = 403;
                response.End();
                return;
            }

            if (!CurrentFolder.CheckAcl(AccessControlRules.FileView))
            {
                response.AddHeader("X-CKFinder-Error", Errors.Unauthorized.ToString());
                response.StatusCode = 403;
                response.End();
                return;
            }

            bool is304 = false;

            string fileName = HttpContext.Current.Request["FileName"];

            string thumbFilePath = Path.Combine(CurrentFolder.ThumbsServerPath, fileName.Replace("/", "\\"));

            if (!Connector.CheckFileName(fileName))
            {
                response.AddHeader("X-CKFinder-Error", Errors.InvalidRequest.ToString());
                response.StatusCode = 403;
                response.End();
                return;
            }

            if (Config.Current.CheckIsHiddenFile(fileName))
            {
                response.AddHeader("X-CKFinder-Error", string.Format("{0} - Hidden folder", Errors.FileNotFound));
                response.StatusCode = 404;
                response.End();
                return;
            }

            // If the thumbnail file doesn't exists, create it now.
            if (!File.Exists(thumbFilePath))
            {
                string sourceFilePath = Path.Combine(CurrentFolder.ServerPath, fileName);

                if (!File.Exists(sourceFilePath))
                {
                    response.AddHeader("X-CKFinder-Error", Errors.FileNotFound.ToString());
                    response.StatusCode = 404;
                    response.End();
                    return;
                }

                ImageTools.ResizeImage(sourceFilePath, thumbFilePath, Config.Current.Thumbnails.MaxWidth, Config.Current.Thumbnails.MaxHeight, true, Config.Current.Thumbnails.Quality);
            }

            var thumbfile = new FileInfo(thumbFilePath);

            string eTag = thumbfile.LastWriteTime.Ticks.ToString("X") + "-" + thumbfile.Length.ToString("X");

            string chachedETag = Request.ServerVariables["HTTP_IF_NONE_MATCH"];
            if (!string.IsNullOrEmpty(chachedETag) && eTag == chachedETag)
                is304 = true;

            if (!is304)
            {
                string cachedTimeStr = Request.ServerVariables["HTTP_IF_MODIFIED_SINCE"];
                if (!string.IsNullOrEmpty(cachedTimeStr))
                {
                    try
                    {
                        DateTime cachedTime = DateTime.Parse(cachedTimeStr);

                        if (cachedTime >= thumbfile.LastWriteTime)
                            is304 = true;
                    }
                    catch
                    {
                        is304 = false;
                    }
                }
            }

            if (is304)
            {
                response.StatusCode = 304;
                response.End();
                return;
            }

            string thumbFileExt = Path.GetExtension(thumbFilePath).TrimStart('.').ToLower();

            if (thumbFilePath == ".jpg")
                response.ContentType = "image/jpeg";
            else
                response.ContentType = "image/" + thumbFileExt;

            response.Cache.SetETag(eTag);
            response.Cache.SetLastModified(thumbfile.LastWriteTime);
            response.Cache.SetCacheability(HttpCacheability.Private);

            response.WriteFile(thumbFilePath);
        }
    }
}