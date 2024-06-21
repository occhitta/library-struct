namespace Occhitta.Libraries.Stream;

/// <summary>
/// <see cref="BinaryLightReader" />検証クラスです。
/// </summary>
[TestFixture]
public class BinaryLightReaderTest : BinaryLightReader {
	#region 非公開ソース定義
	#region メンバー変数定義
	/// <summary>
	/// 読込位置
	/// </summary>
	private int offset;
	/// <summary>
	/// 要素配列
	/// </summary>
	private byte[]? source;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 要素配列を取得します。
	/// </summary>
	/// <value>要素配列</value>
	private byte[] Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.offset = default;
		this.source = default;
		GC.SuppressFinalize(this);
	}
	#endregion 破棄メソッド定義

	#region 内部メソッド定義:ToSource
	/// <summary>
	/// 要素配列へ変換します。
	/// </summary>
	/// <param name="start">開始番号</param>
	/// <param name="count">要素個数</param>
	/// <returns>要素配列</returns>
	private static byte[] ToSource(byte start, int count) {
		var result = new byte[count];
		for (var index = 0; index < count; index ++) {
			result[index] = start ++;
		}
		return result;
	}
	#endregion 内部メソッド定義:ToSource

	#region 内部メソッド定義:ToString
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">要素個数</param>
	/// <returns>要素内容</returns>
	private static string ToString(byte[] source, int offset, int length) {
		var result = new StringBuilder(length * 4);
		for (var index = 0; index < length; index ++) {
			if (index == 0) {
				// 処理なし
			} else if (index % 16 == 0) {
				result.AppendLine();
			} else if (index % 08 == 0) {
				result.Append('-');
			} else {
				result.Append(' ');
			}
			result.Append($"{source[offset + index]:X02}");
		}
		return result.ToString();
	}
	/// <summary>
	/// 要素内容へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <returns>要素内容</returns>
	private static string ToString(byte[] source) =>
		ToString(source, 0, source.Length);
	#endregion 内部メソッド定義:ToString

	#region 内部メソッド定義:ReadData
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	private int ReadData() {
		var source = Source;
		var result = this.offset < source.Length? source[this.offset ++]: -1;
		return result;
	}
	#endregion 内部メソッド定義:ReadData

	#region 実装メソッド定義:BinaryLightReader
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <returns>後続情報</returns>
	int BinaryLightReader.Read() =>
		ReadData();
	/// <summary>
	/// 後続情報を読込みます。
	/// </summary>
	/// <param name="buffer">保存配列</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">保存個数</param>
	/// <returns>保存した個数を返却</returns>
	int BinaryLightReader.Read(byte[] buffer, int offset, int length) {
		for (var index = 0; index < length; index ++) {
			var choose = ReadData();
			if (choose < 0) {
				return index;
			} else {
				buffer[offset + index] = (byte)choose;
			}
		}
		return length;
	}
	#endregion 実装メソッド定義:BinaryLightReader
	#endregion 非公開ソース定義

	#region 検証メソッド定義:Test
	/// <summary>
	/// <see cref="BinaryLightReader.Read(byte[])" />を実行します。
	/// </summary>
	/// <param name="source">検証情報</param>
	/// <param name="buffer">保存配列</param>
	/// <returns>保存した個数を返却</returns>
	private static int Test(BinaryLightReader source, byte[] buffer) =>
		source.Read(buffer);
	/// <summary>
	/// <see cref="BinaryLightReader.Read(byte[])" />を検証します。
	/// </summary>
	/// <param name="startValue">開始情報</param>
	/// <param name="binarySize">保持個数</param>
	/// <param name="bufferSize">保存個数</param>
	/// <param name="expectSize">想定個数</param>
	[TestCase(0x00, 256, 16,  16)]
	public void Test(byte startValue, int binarySize, int bufferSize, int expectSize) {
		this.source = ToSource(startValue, binarySize);
		Assert.That(this.source, Has.Length.EqualTo(binarySize));
		var expectText = ToString(ToSource(startValue, expectSize));
		var bufferData = new byte[bufferSize];
		var actualSize = Test(this, bufferData);
		var actualText = ToString(bufferData, 0, actualSize);
		Assert.Multiple(() => {
			Assert.That(actualSize, Is.EqualTo(expectSize));
			Assert.That(actualText, Is.EqualTo(expectText));
		});
	}
	#endregion 検証メソッド定義:Test
}
