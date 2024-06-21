namespace Occhitta.Libraries.Stream.Binary;

using static Occhitta.Libraries.CommonCode;

/// <summary>
/// <see cref="MemoryBinaryLightReader"/>検証クラスです。
/// </summary>
public class MemoryBinaryLightReaderTest {
	#region 検証メソッド定義:Success
	/// <summary>
	/// <see cref="MemoryBinaryLightReader.Read()" />を検証します。
	/// </summary>
	/// <param name="import">検証情報</param>
	[TestCase("")]
	[TestCase("00")]
	[TestCase("000102030405060708090A0B0C0D0E0F")]
	public void Success1(string import) {
		Assert.That(import.Length % 2, Is.EqualTo(0));
		using var source = new MemoryBinaryLightReader(ToData(import));
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
	/// <see cref="MemoryBinaryLightReader.Read(byte[], int, int)" />を検証します。
	/// </summary>
	/// <param name="import">検証情報</param>
	[TestCase("",                                 1024)]
	[TestCase("00",                               1024)]
	[TestCase("000102030405060708090A0B0C0D0E0F", 1024)]
	[TestCase("000102030405060708090A0B0C0D0E0F",    8)]
	public void Success2(string import, int length) {
		Assert.That(import.Length % 2, Is.EqualTo(0));
		using var source = new MemoryBinaryLightReader(ToData(import));
		var buffer = new byte[length];
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
		using var source = new MemoryBinaryLightReader([0x00, 0x01]);
		((IDisposable)source).Dispose();
		var result = Assert.Throws<ObjectDisposedException>(() => source.Read());
		Assert.Multiple(() => {
			Assert.That(result.Message,        Is.EqualTo("Cannot access a disposed object.\r\nObject name: 'Occhitta.Libraries.Stream.Binary.MemoryBinaryLightReader'."));
			Assert.That(result.ObjectName,     Is.EqualTo("Occhitta.Libraries.Stream.Binary.MemoryBinaryLightReader"));
			Assert.That(result.Source,         Is.EqualTo("Occhitta.Libraries.Struct"));
			Assert.That(result.InnerException, Is.Null,                                                  "InnerException");
			Assert.That(result.Data,           Is.Empty,                                                 "Data");
			Assert.That(result.HelpLink,       Is.Null,                                                  "HelpLink");
			Assert.That(result.HResult,        Is.EqualTo(-2146232798),                                  "HResult");
		});
	}
	#endregion 検証メソッド定義:Failure
}
