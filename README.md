PullAndReleaseTest
==================

Windows Phone 7/8 の xaml で Pull to refresh の実装テスト

問題点、課題
------------

+ ManipulationとMouseのイベントが混在している。なぜかイベントが発生しない。
+ ListBoxExにTemplateを指定すると機能しない(TestPage2でTestPage1のようなTop、Bottomの表示をさせたいができない)+
+ ListBoxのOffsetがピクセルではなく行なのでListBoxExで素早くドラッグしたときに閾値の判定処理が良くない


