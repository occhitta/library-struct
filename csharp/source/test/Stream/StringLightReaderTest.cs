namespace Occhitta.Libraries.Stream;

/// <summary>
/// </summary>
public class StringLightReaderTest : StringLightReader {
	#region メンバー変数定義
	/// <summary>
	/// 要素情報
	/// </summary>
	private int source = 0;
	#endregion メンバー変数定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 実装メソッド定義
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	int StringLightReader.Read() => this.source < 256? this.source ++: -1;
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	int StringLightReader.Read(char[] buffer, int offset, int length) {
		for (var index = 0; index < length; index ++) {
			var choose = this.source < 256? this.source ++: -1;
			if (choose == -1) {
				return index;
			} else {
				buffer[offset + index] = (char)choose;
			}
		}
		return length;
	}
	#endregion 実装メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// <see cref="StringLightReader.Read(char[])" />を検証します。
	/// </summary>
	/// <param name="source">検証情報</param>
	private static void Test(StringLightReader source) {
		var buffer = new char[1024];
		Assert.That(source.Read(buffer), Is.EqualTo(256));
		for (var index = 0; index < buffer.Length; index ++) {
			Assert.That(buffer[index], Is.EqualTo(index < 256? index: 0));
		}
	}
	#endregion 内部メソッド定義

	#region 検証メソッド定義
	/// <summary>
	/// <see cref="StringLightReader.Read(char[])" />を検証します。
	/// </summary>
	[Test]
	public void Test() => Test(this);
	#endregion 検証メソッド定義
}
