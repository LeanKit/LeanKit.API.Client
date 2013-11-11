//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.Enumerations
{
	public enum EventType
	{
		BoardCreation,
		BoardEdit,
		CardBlocked,
		CardCreation,
		CardDeleted,
		CardFieldsChanged,
		CardMove,
		CommentPost,
		UserAssignment,
		UserWipOverride,
		WipOverride,
		AttachmentChange,
		CardMoveToBoard,
		CardMoveFromBoard,
		BoardCardTypesChanged,
		BoardClassOfServiceChanged,
		Unrecognized
	}
}