namespace Occhitta.Libraries.Stream.Binary;

/// <summary>
/// <see cref="StreamBinaryLightReader" />専用検証クラスです。
/// </summary>
[TestFixture]
public class StreamBinaryLightReaderTest {
	#region 内部メソッド定義:ToData/ToText
	/// <summary>
	/// バイナリ情報へ変換します。
	/// </summary>
	/// <param name="source">処理情報</param>
	/// <returns>バイナリ情報</returns>
	private static byte[] ToData(ReadOnlySpan<char> source) {
		var result = new byte[source.Length / 2];
		for (var index = 0; index < result.Length; index ++) {
			result[index] = Byte.Parse(source[(index * 2)..(index * 2 + 2)], System.Globalization.NumberStyles.HexNumber);
		}
		return result;
	}
	/// <summary>
	/// テキスト情報へ変換します。
	/// </summary>
	/// <param name="source">処理情報</param>
	/// <returns>テキスト情報</returns>
	private static string ToText(byte source) =>
		$"{source:X2}";
	/// <summary>
	/// テキスト情報へ変換します。
	/// </summary>
	/// <param name="source">処理情報</param>
	/// <param name="offset">開始位置</param>
	/// <param name="length">要素個数</param>
	/// <returns>テキスト情報</returns>
	private static string ToText(ReadOnlySpan<byte> source, int offset, int length) {
		var result = new StringBuilder(length * 2);
		for (var index = 0; index < length; index ++) {
			result.AppendFormat("{0:X2}", source[offset + index]);
		}
		return result.ToString();
	}
	#endregion 内部メソッド定義:ToData/ToText

	#region 検証メソッド定義:Success
	/// <summary>
	/// <see cref="StreamBinaryLightReader.Read()" />を検証します。
	/// </summary>
	/// <param name="import">検証情報</param>
	[TestCase("")]
	[TestCase("00")]
	[TestCase("000102030405060708090A0B0C0D0E0F")]
	public void Success1(string import) {
		Assert.That(import.Length % 2, Is.EqualTo(0));
		using var stream = new MemoryStream(ToData(import));
		using var source = new StreamBinaryLightReader(stream);
		var result = new StringBuilder();
		while (true) {
			var choose = source.Read();
			if (choose < 0) {
				break;
			} else {
				result.Append(ToText((byte)choose));
			}
		}
		Assert.That(result.ToString(), Is.EqualTo(import));
	}
	/// <summary>
	/// <see cref="StreamBinaryLightReader.Read(byte[], int, int)" />を検証します。
	/// </summary>
	/// <param name="import">検証情報</param>
	[TestCase("")]
	[TestCase("00")]
	[TestCase("000102030405060708090A0B0C0D0E0F")]
	public void Success2(string import) {
		Assert.That(import.Length % 2, Is.EqualTo(0));
		using var stream = new MemoryStream(ToData(import));
		using var source = new StreamBinaryLightReader(stream);
		var buffer = new byte[1024];
		var result = new StringBuilder();
		while (true) {
			var choose = source.Read(buffer, 0, buffer.Length);
			if (choose <= 0) {
				break;
			} else {
				result.Append(ToText(buffer, 0, choose));
			}
		}
		Assert.That(result.ToString(), Is.EqualTo(import));
	}
	#endregion 検証メソッド定義:Success

	#region 検証メソッド定義:Failure
	/// <summary>
	/// 失敗処理を検証します。
	/// </summary>
	[Test]
	public void Failure1() {
		#pragma warning disable CS8625
		var result = Assert.Throws<ArgumentNullException>(static () => new StreamBinaryLightReader(null));
		#pragma warning restore CS8625
		Assert.Multiple(() => {
			Assert.That(result.Message,        Is.EqualTo("Value cannot be null. (Parameter 'source')"), "Message");
			Assert.That(result.ParamName,      Is.EqualTo("source"),                                     "ParamName");
			Assert.That(result.Source,         Is.EqualTo("Occhitta.Libraries.Struct"),                  "Source");
			Assert.That(result.InnerException, Is.Null,                                                  "InnerException");
			Assert.That(result.Data,           Is.Empty,                                                 "Data");
			Assert.That(result.HelpLink,       Is.Null,                                                  "HelpLink");
			Assert.That(result.HResult,        Is.EqualTo(-2147467261),                                  "HResult");
		});
	}
	/// <summary>
	/// 失敗処理を検証します。
	/// </summary>
	[Test]
	public void Failure2() {
		using var stream = new MemoryStream([0x00, 0x01]);
		using var source = new StreamBinaryLightReader(stream);
		((IDisposable)source).Dispose();
		var result = Assert.Throws<ObjectDisposedException>(() => source.Read());
		Assert.Multiple(() => {
			Assert.That(result.Message,        Is.EqualTo("Cannot access a disposed object.\r\nObject name: 'Occhitta.Libraries.Stream.Binary.StreamBinaryLightReader'."));
			Assert.That(result.ObjectName,     Is.EqualTo("Occhitta.Libraries.Stream.Binary.StreamBinaryLightReader"));
			Assert.That(result.Source,         Is.EqualTo("Occhitta.Libraries.Struct"));
			Assert.That(result.InnerException, Is.Null,                                                  "InnerException");
			Assert.That(result.Data,           Is.Empty,                                                 "Data");
			Assert.That(result.HelpLink,       Is.Null,                                                  "HelpLink");
			Assert.That(result.HResult,        Is.EqualTo(-2146232798),                                  "HResult");
		});
	}
	#endregion 検証メソッド定義:Failure
}
