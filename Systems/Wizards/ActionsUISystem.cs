﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invert.Common;
using Invert.Core.GraphDesigner.Systems.GraphUI;
using Invert.Core.GraphDesigner.Systems.GraphUI.api;
using Invert.Core.GraphDesigner.Unity.InspectorWindow;
using UnityEngine;

namespace Invert.Core.GraphDesigner.Unity.Wizards
{
    public class ActionsUISystem : DiagramPlugin, IDrawActionDialog, IDrawActionsPanel
    {

        //TODO WIZARDS Find better way to cache inspectors
        private readonly Dictionary<ActionItem,uFrameMiniInspector> _inspectors = new Dictionary<ActionItem, uFrameMiniInspector>();

        public void DrawActionDialog(IPlatformDrawer platform, Rect bounds, ActionItem item, Action cancel = null)
        {
            if (item == null) return;

            platform.DrawStretchBox(bounds, CachedStyles.WizardSubBoxStyle, 13);

            bounds = bounds.PadSides(15);

            var descriptionHeight = string.IsNullOrEmpty(item.Description) ? 0 : platform.CalculateTextHeight(item.Description, CachedStyles.BreadcrumbTitleStyle, bounds.width) + 60;

            var headerRect = bounds.WithHeight(40);
            var iconRect = bounds.WithSize(41, 41);
            
            var descriptionRect = headerRect.Below(headerRect).WithHeight(descriptionHeight);
            var inspectorRect = bounds.Below(descriptionRect).Clip(bounds);
            var executeButtonRect = new Rect()
                .WithSize(100, 30)
                .InnerAlignWithBottomRight(bounds);

            platform.DrawLabel(headerRect, item.Title, CachedStyles.WizardSubBoxTitleStyle, DrawingAlignment.MiddleCenter);
            platform.DrawImage(iconRect, string.IsNullOrEmpty(item.Icon) ? "CreateEmptyDatabaseIcon" : item.Icon, true);
            platform.DrawLabel(descriptionRect, item.Description, CachedStyles.BreadcrumbTitleStyle, DrawingAlignment.MiddleLeft);

            if (!_inspectors.ContainsKey(item))
            {
                var uFrameMiniInspector = new uFrameMiniInspector(item.Command);
                _inspectors.Add(item, uFrameMiniInspector);
            }

            //Draw generic inspector
            GUILayout.BeginArea(inspectorRect);
                _inspectors[item].Draw();
            GUILayout.EndArea();

            if ( cancel != null)
            {
                platform.DoButton(executeButtonRect.InnerAlignWithBottomLeft(bounds), "Cancel", ElementDesignerStyles.ButtonStyle, cancel);
            }

            platform.DoButton(executeButtonRect, string.IsNullOrEmpty(item.Verb) ? "Create" : item.Verb, ElementDesignerStyles.ButtonStyle, () =>
            {
                InvertApplication.Execute(item.Command);
            });
        }

        //TODO WIZARDS: Add scrolling (drawer needs to be extended to support scrolling / or use native unity stuff)
        public void DrawActionsPanel(IPlatformDrawer platform, Rect bounds, List<ActionItem> actions, Action<ActionItem,Vector2> primaryAction,
            Action<ActionItem,Vector2> secondaryAction = null)
        {
            platform.DrawStretchBox(bounds, CachedStyles.WizardSubBoxStyle, 13);

            bounds = bounds.PadSides(15);
            var headerRect = new Rect(bounds.WithHeight(40));

            platform.DrawLabel(headerRect, "Actions", CachedStyles.WizardSubBoxTitleStyle, DrawingAlignment.TopCenter);

            bounds = bounds.Below(headerRect).Clip(bounds);

            var buttonSize = 100;
            var buttonsPerRow = (int)bounds.width / (int)buttonSize;
            var buttonIndex = 0;
            var padding = (bounds.width % buttonSize) / (buttonsPerRow - 1);
            var itemRect = new Rect().Align(bounds).WithSize(buttonSize, buttonSize);

            foreach (var action in actions)
            {

                platform.DrawStretchBox(itemRect, CachedStyles.WizardActionButtonStyle, 0);

                var action1 = action;
                platform.DoButton(itemRect,"",CachedStyles.ClearItemStyle, m =>
                {
                    primaryAction(action1, m);
                }, m =>
                {
                    if (secondaryAction != null)
                    {
                        secondaryAction(action1, m);
                    }
                });

                var imageRect = itemRect
                    .WithSize(41, 41)
                    .CenterInsideOf(itemRect)
                    .AlignHorisontally(itemRect)
                    .Translate(0, 10);

                var titleRect = itemRect
                    .Below(imageRect)
                    .Clip(itemRect)
                    .Pad(15, 0, 30, 0)
                    .Translate(0, -2);

                platform.DrawImage(imageRect, string.IsNullOrEmpty(action.Icon) ? "CreateEmptyDatabaseIcon" : action.Icon, true);
                platform.DrawLabel(titleRect, action.Title, CachedStyles.WizardActionTitleStyle, DrawingAlignment.MiddleCenter);

                buttonIndex++;

                if (buttonIndex % buttonsPerRow == 0)
                {
                    itemRect = itemRect.Below(itemRect).AlignVertically(bounds).Translate(0, 10);
                }
                else
                {
                    itemRect = itemRect.RightOf(itemRect).Translate(padding, 0);
                }

            }
        }


    }
}