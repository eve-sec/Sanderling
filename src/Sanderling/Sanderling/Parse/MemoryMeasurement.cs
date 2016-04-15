﻿using Bib3;
using BotEngine.Common;
using System.Linq;
using MemoryStruct = Sanderling.Interface.MemoryStruct;
using Bib3.Geometrik;

namespace Sanderling.Parse
{
	public interface IMemoryMeasurement : MemoryStruct.IMemoryMeasurement
	{
		new IShipUiTarget[] Target { get; }

		new IShipUi ShipUi { get; }

		new IModuleButtonTooltip ModuleButtonTooltip { get; }

		new IWindowOverview[] WindowOverview { get; }

		new IWindowInventory[] WindowInventory { get; }

		new IWindowAgentDialogue[] WindowAgentDialogue { get; }

		new INeocom Neocom { get; }

		bool? IsDocked { get; }

		bool? IsUnDocking { get; }
	}

	public partial class MemoryMeasurement
	{
		MemoryStruct.IMemoryMeasurement Raw;

		public IShipUiTarget[] Target { set; get; }

		public IShipUi ShipUi { set; get; }

		public IModuleButtonTooltip ModuleButtonTooltip { set; get; }

		public IWindowOverview[] WindowOverview { set; get; }

		public IWindowInventory[] WindowInventory { set; get; }

		public IWindowAgentDialogue[] WindowAgentDialogue { set; get; }

		public INeocom Neocom { set; get; }

		public bool? IsDocked { private set; get; }

		public bool? IsUnDocking { private set; get; }
	}

	public partial class MemoryMeasurement : IMemoryMeasurement
	{
		public string UserDefaultLocaleName => Raw?.UserDefaultLocaleName;

		public MemoryStruct.IInSpaceBracket[] InflightBracket => Raw?.InflightBracket;

		public MemoryStruct.IUIElementText[] AbovemainMessage => Raw?.AbovemainMessage;

		public MemoryStruct.PanelGroup[] AbovemainPanelEveMenu => Raw?.AbovemainPanelEveMenu;

		public MemoryStruct.PanelGroup[] AbovemainPanelGroup => Raw?.AbovemainPanelGroup;

		public MemoryStruct.IUIElement InfoPanelButtonIncursions => Raw?.InfoPanelButtonIncursions;

		public MemoryStruct.IUIElement InfoPanelButtonCurrentSystem => Raw?.InfoPanelButtonCurrentSystem;

		public MemoryStruct.IUIElement InfoPanelButtonMissions => Raw?.InfoPanelButtonMissions;

		public MemoryStruct.IUIElement InfoPanelButtonRoute => Raw?.InfoPanelButtonRoute;

		public MemoryStruct.IInfoPanelSystem InfoPanelCurrentSystem => Raw?.InfoPanelCurrentSystem;

		public MemoryStruct.IInfoPanelMissions InfoPanelMissions => Raw?.InfoPanelMissions;

		public MemoryStruct.IInfoPanelRoute InfoPanelRoute => Raw?.InfoPanelRoute;

		public MemoryStruct.IMenu[] Menu => Raw?.Menu;

		public MemoryStruct.IContainer[] Tooltip => Raw?.Tooltip;

		MemoryStruct.INeocom MemoryStruct.IMemoryMeasurement.Neocom => Neocom;

		MemoryStruct.IShipUi MemoryStruct.IMemoryMeasurement.ShipUi => ShipUi;

		public MemoryStruct.IWindow SystemMenu => Raw?.SystemMenu;

		MemoryStruct.IShipUiTarget[] MemoryStruct.IMemoryMeasurement.Target => Target;

		public MemoryStruct.IContainer[] Utilmenu => Raw?.Utilmenu;

		public string VersionString => Raw?.VersionString;

		public MemoryStruct.WindowAgentBrowser[] WindowAgentBrowser => Raw?.WindowAgentBrowser;

		MemoryStruct.IWindowAgentDialogue[] MemoryStruct.IMemoryMeasurement.WindowAgentDialogue => WindowAgentDialogue;

		public MemoryStruct.WindowChatChannel[] WindowChatChannel => Raw?.WindowChatChannel;

		public MemoryStruct.IWindowDroneView[] WindowDroneView => Raw?.WindowDroneView;

		public MemoryStruct.WindowFittingMgmt[] WindowFittingMgmt => Raw?.WindowFittingMgmt;

		public MemoryStruct.WindowShipFitting[] WindowShipFitting => Raw?.WindowShipFitting;

		MemoryStruct.IWindowOverview[] MemoryStruct.IMemoryMeasurement.WindowOverview => WindowOverview;

		MemoryStruct.IWindowInventory[] MemoryStruct.IMemoryMeasurement.WindowInventory => WindowInventory;

		public MemoryStruct.WindowItemSell[] WindowItemSell => Raw?.WindowItemSell;

		public MemoryStruct.WindowMarketAction[] WindowMarketAction => Raw?.WindowMarketAction;

		public MemoryStruct.IWindow[] WindowOther => Raw?.WindowOther;

		public MemoryStruct.WindowPeopleAndPlaces[] WindowPeopleAndPlaces => Raw?.WindowPeopleAndPlaces;

		public MemoryStruct.WindowRegionalMarket[] WindowRegionalMarket => Raw?.WindowRegionalMarket;

		public MemoryStruct.IWindowSelectedItemView[] WindowSelectedItemView => Raw?.WindowSelectedItemView;

		public MemoryStruct.WindowStack[] WindowStack => Raw?.WindowStack;

		public MemoryStruct.IWindowStation[] WindowStation => Raw?.WindowStation;

		public MemoryStruct.IWindowSurveyScanView[] WindowSurveyScanView => Raw?.WindowSurveyScanView;

		public MemoryStruct.WindowTelecom[] WindowTelecom => Raw?.WindowTelecom;

		MemoryStruct.IContainer MemoryStruct.IMemoryMeasurement.ModuleButtonTooltip => ModuleButtonTooltip;

		public Vektor2DInt ScreenSize => Raw?.ScreenSize ?? default(Vektor2DInt);

		MemoryMeasurement()
		{
		}

		public MemoryMeasurement(MemoryStruct.IMemoryMeasurement raw)
		{
			this.Raw = raw;

			if (null == raw)
				return;

			Culture.InvokeInParseCulture(() =>
			{
				Target = raw?.Target?.Select(ShipUiExtension.Parse)?.ToArray();

				ModuleButtonTooltip = raw?.ModuleButtonTooltip?.ParseAsModuleButtonTooltip();

				WindowOverview = raw?.WindowOverview?.Select(OverviewExtension.Parse)?.ToArray();

				WindowInventory = raw?.WindowInventory?.Select(InventoryExtension.Parse)?.ToArray();

				WindowAgentDialogue = raw?.WindowAgentDialogue?.Select(DialogueMissionExtension.Parse)?.ToArray();

				ShipUi = raw?.ShipUi?.Parse();

				var SetWindowStation = raw?.WindowStation;

				if (!SetWindowStation.IsNullOrEmpty())
				{
					IsDocked = true;
				}

				if (null != ShipUi ||
					(raw?.WindowOverview?.WhereNotDefault()?.Any() ?? false))
				{
					IsDocked = false;
				}

				if (!(IsDocked ?? true))
				{
					IsUnDocking = false;
				}

				if (SetWindowStation?.Any(windowStationLobby => windowStationLobby?.LabelText?.Any(labelText =>
					 labelText?.Text?.RegexMatchSuccess(@"abort\s*undock|undocking") ?? false) ?? false) ?? false)
				{
					IsUnDocking = true;
				}

				Neocom = raw?.Neocom?.Parse();
			});
		}
	}
}
