using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class TagController
{
    public IReadOnlyReactiveDictionary<string, int> tags => m_TagCountMap;

    private ReactiveDictionary<string, int> m_TagCountMap = new();

    public bool Contains(string _tag)
    {
        return m_TagCountMap.ContainsKey(_tag);
    }

    public bool ContainsAny(IEnumerable<string> _tags)
    {
        return _tags.Any(m_TagCountMap.ContainsKey);
    }

    public bool ContainsAll(IEnumerable<string> _tags)
    {
        return _tags.All(m_TagCountMap.ContainsKey);
    }

    public bool SatisfiesRequirements(IEnumerable<string> _mustBePresentTags, IEnumerable<string> _mustBeAbsentTags)
    {
        return ContainsAll(_mustBePresentTags) && !ContainsAny(_mustBeAbsentTags);
    }

    public void AddTag(string _tag)
    {
        if (m_TagCountMap.ContainsKey(_tag) == false)
        {
            m_TagCountMap.Add(_tag, 1);
            return;
        }

        ++m_TagCountMap[_tag];
    }

    public void RemoveTag(string _tag)
    {
        if (m_TagCountMap.ContainsKey(_tag) == false)
        {
            Debug.LogError($"삭제를 시도하는 태그 \"{_tag}\"가 존재하지 않습니다.");
        }

        --m_TagCountMap[_tag];
        if (m_TagCountMap[_tag] == 0)
        {
            m_TagCountMap.Remove(_tag);
        }
    }
}