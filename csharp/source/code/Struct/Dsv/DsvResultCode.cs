namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// DSV結果構築処理移譲体です。
/// </summary>
/// <typeparam name="TItem">項目種別</typeparam>
/// <typeparam name="TData">結果種別</typeparam>
/// <param name="valueList">項目集合</param>
/// <returns>結果種別</returns>
public delegate TData DsvResultCode<in TItem, out TData>(IEnumerable<TItem> valueList);
