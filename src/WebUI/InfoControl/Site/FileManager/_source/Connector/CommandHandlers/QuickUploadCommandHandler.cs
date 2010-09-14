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
using System.Collections.Generic;
using System.Text;

namespace CKFinder.Connector.CommandHandlers
{
	internal class QuickUploadCommandHandler : FileUploadCommandHandler
	{
		public QuickUploadCommandHandler()
			: base()
		{
		}

		protected override string GetJavaScriptCode( int errorNumber, string fileName, string fileUrl )
		{
			switch ( errorNumber )
			{
				case Errors.None:
				case Errors.UploadedFileRenamed:
					return "window.parent.OnUploadCompleted(" + errorNumber + ",'" + fileUrl.Replace( "'", "\\'" ) + "','" + fileName.Replace( "'", "\\'" ) + "') ;";
				default:
					return "window.parent.OnUploadCompleted(" + errorNumber + ") ;";
			}
		}
	}
}
