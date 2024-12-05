# Unity FPS Test

## 新規プロジェクトから始める場合

Unity 2022.3 でテストしています。

1. 3D プロジェクトを作成し、Unity Editor 上で `fps_test.unitypackage` を Assets フォルダにドラッグアンドドロップしてください。
2. `FpsTest` フォルダ内に `fps_test` マップがあるので開きます。
3. 開くと Text Mesh Pro のインポートダイアログが出るのでインポートしてください。

## このリポジトリのプロジェクトを使う場合

Download ZIP して開いてください。

## State パターンを使っている箇所のダイアグラム

![](/docs/StatesDiagram.png)

- 破線は継承・実装関係です。`IMovementState` をインターフェースとして、Base, Default, Jump の State があります。
- `CharacterMovement` は各 MovementState から利用はされていますが、逆に State のことを知ることはありません。
- `CharacterStatus` は `MovementStates`, `CharacterMovement` の両方を参照し状態を確認できます。
