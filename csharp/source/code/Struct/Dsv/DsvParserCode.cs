namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// DSV項目解析処理移譲体です。
/// </summary>
/// <typeparam name="TValue">要素種別</typeparam>
/// <param name="sourceData">要素情報</param>
/// <param name="columnCode">情報番号</param>
/// <param name="recordCode">要素番号</param>
/// <returns>項目情報</returns>
public delegate TValue DsvParserCode<out TValue>(string sourceData, int columnCode, int recordCode);
