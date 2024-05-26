using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public Transform slot;
    public GameObject item;

    public float working_time = 10f;
    public bool ready = false;
    public bool working = false;
    public bool waiting = true;

    public float current_timer;

    Animator m_Animator;

    private bool once = false;

    public class TextField
    {
        public string gravirovka;
    }

    public TextField textField;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        LoadJson();
    }

    // Update is called once per frame
    void Update()
    {
        if (working)
        {
            current_timer -= Time.deltaTime;
            if (current_timer <= working_time + 3 && once)
            {
                item.GetComponentInChildren<TextMeshPro>().text = this.textField.gravirovka;
            }
            if (current_timer <= 3 && once)
            {
                m_Animator.ResetTrigger("close");
                m_Animator.SetTrigger("open");
                once = false;
            }
            if (current_timer <= 0)
            {
                working = false;
                ready = true;
            }
        }
    }

    public void GrabItem()
    {
        item = null;
        ready = false;
        waiting = true;
    }

    public void PutItem(GameObject item)
    {
        this.item = item;
        StartWorking();
    }

    void StartWorking()
    {
        current_timer = working_time + 6;
        working = true;
        waiting = false;
        once = true;
        m_Animator.ResetTrigger("open");
        m_Animator.SetTrigger("close");
    }

    void LoadJson()
    {
        // �������� ����� �� ��������
        TextAsset jsonFile = Resources.Load<TextAsset>("data");

        if (jsonFile != null)
        {
            // ������ JSON ������ �� �����
            string jsonText = jsonFile.text;

            // �������������� � ������������ ������
            textField = JsonConvert.DeserializeObject<TextField>(jsonText);

        }
        else
        {
            textField.gravirovka = "Temp";
            Debug.LogError("�� ������� ��������� JSON ����.");
        }
    }
}
